(function () {

    var controllers = angular.module("hrmsUserInfoControllers", ["hrmsUserInfoServices"]);

    controllers.controller("UserInfoController", ["$scope", "UserLogResource",
        function ($scope, UserLogResource) {
            var today = new Date();

            $scope.startYear = 2013;
            $scope.endYear = today.getFullYear();
            $scope.year = today.getFullYear().toString();
            $scope.month = (today.getMonth() + 1).toString();
            $scope.date = null;
            $scope.dates = [];

            $scope.refreshDates = function () {
                if ($scope.year === "" || $scope.month === "") {
                    $scope.date = null;
                    $scope.dates = [];
                } else {
                    $scope.date = new Date($scope.year, $scope.month - 1, 1);
                    $scope.dates = UserLogResource.query($scope.date);
                }
            };

            $scope.refreshDates();
        }
    ]);

})();
