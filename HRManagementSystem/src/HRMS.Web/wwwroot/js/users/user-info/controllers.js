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
            $scope.records = [];

            $scope.refreshDates = function () {
                if ($scope.year === "" || $scope.month === "") {
                    $scope.date = null;
                    $scope.records = [];
                } else {
                    $scope.date = new Date($scope.year, $scope.month - 1, 1);
                    console.log($scope.date);
                    var month = $scope.year + '-' + $scope.month + '-1';
                    console.log($scope.empId);
                    var empId = $scope.empId;
                    UserLogResource.query({ empId: empId, month: month }, function (records) {
                        $scope.records = records;
                        console.log(records);
                    });
                    setTimeout(function () {
                        console.log($scope.records);
                    }, 5000);
                    
                }
            };
            $scope.$watch('empId', function () {
                $scope.refreshDates();
            })
            
        }
    ]);

})();
