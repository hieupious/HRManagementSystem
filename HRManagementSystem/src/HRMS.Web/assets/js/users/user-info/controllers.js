(function () {

    var controllers = angular.module("hrmsUserInfoControllers", ["hrmsUserInfoServices"]);

    controllers.controller("UserInfoController", ["$scope", "UserLogResource",
        function ($scope, UserLogResource) {
            var today = new Date();

            $scope.startYear = 2013;
            $scope.endYear = today.getFullYear();
            $scope.year = today.getFullYear().toString();
            $scope.month = (today.getMonth() + 1).toString();
            $scope.dates = [];

            $scope.refreshDates = function () {
                if ($scope.year === "" || $scope.month === "") {
                    $scope.dates = [];
                } else {
                    $scope.dates = UserLogResource.query(new Date($scope.year, $scope.month - 1, 1));
                }
            };

            $scope.refreshDates();
        }
    ]);

})();
