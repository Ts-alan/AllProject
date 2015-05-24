
'use strict';

CCPApp.directive("serverErrors", [function () {

    var directive = {
        link: link,
        restrict: "A",
        require: 'ngModel'
    };
    return directive;

    function link(scope, element, attrs, ngModelCtrl) {
        scope.$watch(function () {
            return scope.dataErrors;
        }, function (newVal, oldVal) {
            if (oldVal != newVal) {
                var errors = scope.dataErrors[ngModelCtrl.$name];
                if (errors) {
                    ngModelCtrl.$setValidity("server", false);
                    ngModelCtrl.$error.errorContent = "<ul class=\"error\">";
                    angular.forEach(errors, function (error) {
                        ngModelCtrl.$error.errorContent += "<li>" + error + "</li>";
                    });
                    var x = attrs;
                    ngModelCtrl.$error.errorContent += "</ul>";
                }
            }
        },true);

        scope.$watch(function () {
            return ngModelCtrl.$viewValue;
        }, function () {
            if (!ngModelCtrl.$valid) {
                angular.forEach(ngModelCtrl.$error, function (item, b) {
                     if (b != "errorContent") {
                         ngModelCtrl.$setValidity(b, true);
                     } else {
                         ngModelCtrl.$error.errorContent = "";
                     }   
                 });
            };
            
        });
    }
}]);