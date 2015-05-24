'use strict';

CCPApp.factory('dataService', ['$http', function ($http) {
    return {
        url: "http://"+ window.location.hostname + apiPort, //from web.config
        post: function (type, data, headers) {
            if (headers) {
                return $http({
                    url: this.url + type,
                    method: "POST",
                    data: data,
                    headers: headers
                });
            }
            return $http.post(this.url + type, data);
        },

        get: function (type, data, headers) {
            if (headers) {
                return $http({
                    url: this.url + type,
                    method: "Get",
                    params: {
                        data: data
                    },
                    headers: headers
                });
            }
            return $http({
                url: this.url + type,
                method: "Get",
                params: {
                    data: data
                }
            });
            
        },

        delete: function (type, data) {
            return $http.delete(this.url + type, data);
        }
    }
}]);