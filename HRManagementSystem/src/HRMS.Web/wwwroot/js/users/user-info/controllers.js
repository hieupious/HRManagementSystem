(function () {

    var controllers = angular.module("hrmsUserInfoControllers", ["hrmsUserInfoServices"]);

    controllers.controller("UserInfoController", ["$scope", "UserLogResource",
        function ($scope, UserLogResource) {
            $scope.dates = UserLogResource.query(new Date());
        }
    ]);

})();
