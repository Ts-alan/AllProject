
CCPApp.factory('authInterceptorService', ['$q', 'localStorageService', '$injector', function ($q, localStorageService, $injector) {

    var authInterceptorServiceFactory = {};

    var request = function (config) {

        config.headers = config.headers || {};

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            if (!(config.headers["Content-Type"] && config.headers["Content-Type"] == 'application/x-www-form-urlencoded')) {
                config.headers.Authorization = 'Bearer ' + authData.token;
                config.headers["X-Requested-With"] = "XMLHttpRequest";
            }
            if (config.url.indexOf('api/token') < 0) {
                var authorizationService = $injector.get('authService');
                if ((new Date() - new Date(authorizationService.refreshTokenExpireDate)) / 60000 > 5) {
                    authorizationService.refreshTokenExpireDate = new Date();
                    authorizationService.refreshToken().then(function () {
                         return config;
                    });
                }

            }
        }

        return config;
    }

    var responseError = function (rejection) {
        if (rejection.status === 401) {
            var authorizationService = $injector.get('authService');
            var state = $injector.get('$state');
            authorizationService.logOut();
            state.go('Login');
        }
        return $q.reject(rejection);
    }

    authInterceptorServiceFactory.request = request;
    authInterceptorServiceFactory.responseError = responseError;

    return authInterceptorServiceFactory;
}]);