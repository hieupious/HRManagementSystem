(function () {

    var controllers = angular.module("hrmsUserInfoControllers", ["hrmsUserInfoServices"]);

    controllers.controller("UserInfoController", ["$scope", "$filter", "UserLogResource",
        function ($scope, $filter, UserLogResource) {
            var today = new Date();

            $scope.startYear = 2013;
            $scope.endYear = today.getFullYear();
            $scope.year = today.getFullYear().toString();
            $scope.month = (today.getMonth() + 1).toString();
            $scope.date = null;
            $scope.records = [];

            $scope.refreshRecords = function () {
                if ($scope.year === "" || $scope.month === "") {
                    $scope.date = null;
                    $scope.records = [];
                } else {
                    $scope.records = [];
                    $scope.date = new Date($scope.year, $scope.month - 1, 1);
                    var month = $filter('date')($scope.date, 'yyyy-MM-dd');
                    var empId = $scope.empId;
                    UserLogResource.query({ empId: empId, month: month }, function (records) {
                        $scope.records = records;
                    });
                }
            };
            $scope.$watch('empId', function () {
                $scope.refreshRecords();
            })

        }
    ]);

})();
