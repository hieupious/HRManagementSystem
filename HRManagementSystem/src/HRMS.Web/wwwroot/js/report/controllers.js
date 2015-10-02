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
                    $scope.date = new Date($scope.year, $scope.month - 1, 1);
                    var month = $scope.year + '-' + '8' + '-1';
                    console.log(month);
                    ReportResource.query({ month: month }, function (records) {
                        $scope.users = records;
                        console.log($scope.users);
                    });

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
