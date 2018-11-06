(function () {
    'use strict';

    ehrStateConfig.$inject = ['$stateProvider', '$urlRouterProvider'];
    function ehrStateConfig($stateProvider, $urlRouterProvider) {
        console.log('fire ehrStateConfig');

        // Login
        $stateProvider.state({
            name: 'login',
            url: '/login',
            templateUrl: AppConfig.EHRBase + 'account/login.html',
            controller: 'ehrLoginController as loginCtrl'
        });

        // Logout
        $stateProvider.state({
            name: 'logout',
            url: '/logout',
            templateUrl: AppConfig.EHRBase + 'account/logout.html',
            controller: 'ehrLogoutController as logoutCtrl'
        });

        // Change Database  
        $stateProvider.state({
            name: 'changedb',
            url: '/changedb',
            templateUrl: AppConfig.EHRBase + 'account/changedb.html',
            controller: 'ehrAccountController as acctCtrl'//,
            /*
            resolve: {
                ctx: ['ApiService', 'AuthService', function (ApiService, AuthService) {
                    var resp = ApiService.getEntity('User', AuthService.getCurrentUser().Id);
                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }]
            }
            */
        });

        // Initialize
        $stateProvider.state({
            name: 'initialize',
            url: '/initialize',
            templateUrl: AppConfig.EHRBase + 'account/initialize.html',
            controller: 'ehrInitializeController as initCtrl'
        });

        // Home
        $stateProvider.state({
            name: 'home',
            url: '/home',
            templateUrl: AppConfig.EHRBase + 'account/home.html'
        });

        // Account
        $stateProvider.state({
            name: 'account',
            url: '/account',
            templateUrl: AppConfig.EHRBase + 'account/account.html',
            controller: 'ehrAccountController as acctCtrl'//,
            /*
            resolve: {
                ctx: ['ApiService', 'AuthService', function (ApiService, AuthService) {
                    var resp = ApiService.getEntity('User', AuthService.getCurrentUser().Id);
                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }]
            }
            */
        });

        // Subscriber: List
        $stateProvider.state({
            name: 'subscribers',
            url: "/subscribers",
            templateUrl: AppConfig.EHRBase + 'subscribers/subscriber-list.html',
            controller: 'ehrSubscriberListController as subListCtrl'//,
            /*
            resolve: {
                entityList: ['ApiService', function (ApiService) {
                    var resp = ApiService.getEntityList('Subscribers', 'subscriber');
                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }]//,
            }
            */
        });

        // Subscriber: Edit
        $stateProvider.state({
            name: 'subscriber-edit',
            url: 'subscriber/:id',
            templateUrl: AppConfig.EHRBase + 'subscribers/subscriber-edit.html',
            controller: 'ehrSubscriberEditController as subEditCtrl',
            resolve: {
                entity: ['$stateParams', 'ApiService', function ($stateParams, ApiService) {
                    var resp = ApiService.getEntity('Subscriber', $stateParams.id);
                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }]//,
                /*
                payerList: ['ApiService', function (ApiService) {
                    var resp = ApiService.getEntityList('Payers', 'payer');
                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    })
                }]
                */
            }
        });

        // Claim: List
        $stateProvider.state({
            name: 'claims',
            url: "/claims",
            templateUrl: AppConfig.EHRBase + 'claims/claim-list.html',
            controller: 'ehrClaimListController as clmListCtrl'//,
            /*
            resolve: {
                entityList: ['ApiService', function (ApiService) {
                    var resp = ApiService.getEntityList('Claims', 'claim');
                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }]
            }
            */
        });

        // Claim: Edit
        $stateProvider.state({
            name: 'claim-edit',
            url: 'claim/:subscriberId/:claimId',
            templateUrl: AppConfig.EHRBase + 'claims/claim-edit.html',
            controller: 'ehrClaimEditController as clmEditCtrl',
            resolve: {
                subscriber: ['$stateParams', 'ApiService', function ($stateParams, ApiService) {
                    var resp = null;

                    resp = ApiService.getEntity('Subscriber',
                        ($stateParams.subscriberId && $stateParams.subscriberId > 0) == true ?
                            $stateParams.subscriberId : null
                    );

                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }],
                claim: ['$stateParams', 'ApiService', function ($stateParams, ApiService) {
                    var resp = null;
                    var cId = ($stateParams.claimId && $stateParams.claimId > 0) ? $stateParams.claimId : null;

                    resp = ApiService.getEntity('Claim', cId);

                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }],
                history: ['$stateParams', 'ApiService', function ($stateParams, ApiService) {
                    var resp = null;

                    resp = ApiService.getClaimHistory($stateParams.subscriberId );

                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }]
            }
        });

        // Batch: List
        $stateProvider.state({
            name: 'batches',
            url: "/batches",
            templateUrl: AppConfig.EHRBase + 'batches/batch-list.html',
            controller: 'ehrBatchListController as batchListCtrl',
            resolve: {
                entityList: ['ApiService', function (ApiService) {
                    var resp = ApiService.getEntityList('Batches', 'batch');
                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }]
            }
        });

        // Batch: Edit
        $stateProvider.state({
            name: 'batch-edit',
            url: 'batch/:id',
            templateUrl: AppConfig.EHRBase + 'batches/batch-edit.html',
            controller: 'ehrBatchEditController as batchEditCtrl',
            resolve: {
                entity: ['$stateParams', 'ApiService', function ($stateParams, ApiService) {
                    var resp = ApiService.getEntity('Batch', $stateParams.id);
                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }],
                claims: ['$stateParams', 'ApiService', function ($stateParams, ApiService) {
                    var resp = ApiService.getBatchClaims($stateParams.id);
                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }]
            }
        });

        // Reports
        $stateProvider.state({
            name: 'reports',
            url: "/reports",
            templateUrl: AppConfig.EHRBase + 'reports/reports.html',
            controller: 'ehrReportController as reportsCtrl'//,
            /*
            resolve: {
                entityList: ['ApiService', function (ApiService) {
                    var resp = ApiService.getEntityList('Batches', 'batch');
                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }]
            }
            */
        });

        // Setup
        $stateProvider.state({
            name: 'setup',
            url: 'setup',
            templateUrl: AppConfig.EHRBase + 'setup/setup.html',
            controller: 'ehrSetupController as setupCtrl'
        });

        // Payment: List
        $stateProvider.state({
            name: 'payments',
            url: "/payments",
            templateUrl: AppConfig.EHRBase + 'payments/payments-list.html',
            controller: 'ehrPaymentListController as pmtListCtrl'
        });

        // Payment: Edit
        $stateProvider.state({
            name: 'payment-edit',
            url: 'payment/:subscriberId/:claimId',
            templateUrl: AppConfig.EHRBase + 'payments/payment-edit.html',
            controller: 'ehrPaymentEditController as pmtEditCtrl',
            resolve: {
                claim: ['$stateParams', 'ApiService', function ($stateParams, ApiService) {
                    var resp = null;
                    var cId = ($stateParams.claimId && $stateParams.claimId > 0) ? $stateParams.claimId : null;

                    resp = ApiService.getEntity('Claim', cId);

                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }],
                history: ['$stateParams', 'ApiService', function ($stateParams, ApiService) {
                    var resp = null;

                    resp = ApiService.getClaimHistory($stateParams.subscriberId);

                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }]
            }
        });

        // Schedule
        $stateProvider.state({
            name: 'schedule',
            url: '/schedule',
            templateUrl: AppConfig.EHRBase + 'schedule/schedule.html',
            controller: 'ehrScheduleController as schedCtrl'
        });

        function prepareSingleEntity(response) {
            var result = {};

            console.log('prepareSingleEntity');
            console.log(response);

            return result;
        }
 
    }

    angular.module('ehrApp').config(ehrStateConfig);
})();