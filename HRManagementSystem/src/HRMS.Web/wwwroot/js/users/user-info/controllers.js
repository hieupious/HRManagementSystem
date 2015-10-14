(function () {

    var controllers = angular.module("hrmsUserInfoControllers", ["hrmsUserInfoServices"]);

    controllers.controller("UserInfoController", ["$scope", "$filter", "$http", "UserLogResource",
        function ($scope, $filter, $http, UserLogResource) {
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

            $scope.GetApproval = function (record) {
                record.editMode = false;
                record.ApproverId = record.Approver.Id;
                console.log(record);
                $http.put("/api/Users/" + $scope.empId, record);
            }

            //$scope.managers = [{ id: 1, name: 'Mr. Khanh' }, { id: 2, name: 'Mr. Shane' }, { id: 3, name: 'Ms Suong' }, { id: 4, name: 'Mr Son' }];
            $http.get("/api/Users/ManagerList/").success(function (response) {
                $scope.managers = response;
            });
        }
    ]);

})();
