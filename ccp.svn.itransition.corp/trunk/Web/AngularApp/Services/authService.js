
CCPApp.factory('authService', ['$q', '$state', 'localStorageService', 'dataService', function ($q, $state, localStorageService, dataService) {

    var authServiceFactory = {};

    var authentication = {
        userName: "",
        firstName: "",
        lastName: "",
        role: "",
    };

    var refreshTokenExpireDate = new Date();

    var areas = {}

    var returnPage = "";

    var login = function (loginData) {

        var deferred = $q.defer();

        var data = {
            grant_type: "password",
            username: loginData.Email,
            password: loginData.Password,
            client_id: "ngAuthApp"
        }
        var authorizationData = localStorageService.get('authorizationData');
        var areasData = localStorageService.get('areasData');
        if (authorizationData == null || areasData == null) {
            if (authorizationData != null) {
                data.username = authorizationData.username;
                data.password = authorizationData.password;
            }
            dataService.post('token', $.param(data), { 'Content-Type': 'application/x-www-form-urlencoded' }).success(function (response, r) {

                localStorageService.set('authorizationData', { token: response.access_token, userName: response.email, firstName: response.firstName, lastName: response.lastName, role: response.role, refreshToken: response.refresh_token, refreshTokenExpireDate: response.refreshTokenExpireDate });
                localStorageService.set('areasData', { areas: response.areas });
                authentication.userName = response.email;
                authentication.firstName = response.firstName;
                authentication.lastName = response.lastName;
                authentication.role = response.role;
                areas = JSON.parse(response.areas);
                refreshTokenExpireDate = new Date(response.refreshTokenExpireDate);
                deferred.resolve();

            }).error(function (err) {
                logOut();
                $state.go('Login');
                deferred.reject(err);
            });
        } else {
            fillAuthData();
            deferred.resolve();
        }

        return deferred.promise;

    };

    var isAuthenticated = function() {
        return localStorageService.get('authorizationData')!=null;
    }

    var logOut = function () {

        localStorageService.clearAll();
        authentication.userName = "";
        authentication.firstName = "";
        authentication.lastName = "";
        authentication.role = "";
        areas = "";
        refreshTokenExpireDate = new Date();
    };

    var isAllowedArea = function (state) {
        var isAllowed = false;
        angular.forEach(areas, function (item) {
            if (state == item.AreaName) {
                angular.forEach(item.Roles, function (i) {
                    if (authentication.role === i) {
                        isAllowed = true;
                    }
                });
            }
        });
        return isAllowed;
    }

    var fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        var areasData = localStorageService.get('areasData');
        if (authData) {
            authentication.userName = authData.userName;
            authentication.firstName = authData.firstName;
            authentication.lastName = authData.lastName;
            authentication.role = authData.role;
            refreshTokenExpireDate = authData.refreshTokenExpireDate;
        }
        if (areasData && areasData.areas) {
            areas = JSON.parse(areasData.areas);
        }

    }

    var refreshToken = function () {
        var deferred = $q.defer();

        var authData = localStorageService.get('authorizationData');

        if (authData) {

            var data = {
                grant_type: "refresh_token",
                refresh_token: authData.refreshToken,
                client_id: "ngAuthApp"
            }

            dataService.post('token', $.param(data), { 'Content-Type': 'application/x-www-form-urlencoded' }).success(function (response, r) {

                localStorageService.remove('authorizationData');
                localStorageService.remove('areasData');
                localStorageService.set('authorizationData', { token: response.access_token, userName: response.email, firstName: response.firstName, lastName: response.lastName, role: response.role, refreshToken: response.refresh_token, refreshTokenExpireDate: response.refreshTokenExpireDate });
                localStorageService.set('areasData', { areas: response.areas });
                authentication.userName = response.email;
                authentication.firstName = response.firstName;
                authentication.lastName = response.lastName;
                authentication.role = response.role;
                areas = JSON.parse(response.areas);
                deferred.resolve(response);

            }).error(function (err, status) {
                localStorageService.remove('areasData');
                logOut();
                $state.go('Login');
                deferred.reject(err);
            });
        }
        return deferred.promise;
    };

    authServiceFactory.login = login;
    authServiceFactory.logOut = logOut;
    authServiceFactory.isAuthenticated = isAuthenticated;
    authServiceFactory.fillAuthData = fillAuthData;
    authServiceFactory.authentication = authentication;
    authServiceFactory.returnPage = returnPage;
    authServiceFactory.areas = areas;
    authServiceFactory.refreshTokenExpireDate = refreshTokenExpireDate;
    authServiceFactory.isAllowedArea = isAllowedArea;
    authServiceFactory.refreshToken = refreshToken;
    return authServiceFactory;
}]);