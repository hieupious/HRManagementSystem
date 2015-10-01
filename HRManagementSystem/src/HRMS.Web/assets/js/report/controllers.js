(function () {

    var controllers = angular.module("hrmsReportControllers", ["hrmsReportServices"]);

    controllers.controller("ReportController", ["$scope", "ReportResource",
        function ($scope, ReportResource) {
            var today = new Date();

            $scope.startYear = 2013;
            $scope.endYear = today.getFullYear();
            $scope.year = today.getFullYear().toString();
            $scope.month = (today.getMonth() + 1).toString();
            $scope.date = null;
            $scope.users = [];

            $scope.refreshUsers = function () {
                if ($scope.year === "" || $scope.month === "") {
                    $scope.date = null;
                    $scope.users = [];
                } else {
                    $scope.date = new Date($scope.year, $scope.month -1, 1);
                    $scope.users = ReportResource.query($scope.date);
                }
                $.each($scope.users, function (userIndex, user) {
                    user.totalLackTime = 0;
                    $.each(user.Dates, function (dateIndex, date) {
                        user.totalLackTime += date.LackTime;
                    });
                });
            };

            $scope.refreshUsers();
        }
    ]);

})();
