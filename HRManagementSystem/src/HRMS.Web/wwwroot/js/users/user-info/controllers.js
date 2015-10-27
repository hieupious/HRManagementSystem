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
            $scope.loading = false;
            $scope.refreshRecords = function () {
                if ($scope.year === "" || $scope.month === "") {
                    $scope.date = null;
                    $scope.records = [];
                } else {
                    $scope.loading = true;
                    $scope.records = [];
                    $scope.date = new Date($scope.year, $scope.month - 1, 1);
                    var month = $filter('date')($scope.date, 'yyyy-MM-dd');
                    var empId = $scope.empId;
                    UserLogResource.query({ empId: empId, month: month }, function (records) {
                        $scope.records = records;
                        $scope.loading = false;
                    });
                }
            };
            $scope.$watch('empId', function () {
                $scope.refreshRecords();
            })

            $scope.GetApproval = function (record) {
                if (record.GetApprovedReason == null || record.GetApprovedReason == "") {
                    alert('Please enter your reason to get approval');
                    return;
                }
                if (record.Approver == null) {
                    alert("Please select Approver.");
                    return;
                }
                record.editMode = false;

                record.ApproverId = record.Approver.Id;

                $http.put("/api/Users/" + $scope.empId, record);
            }

            $http.get("/api/Users/ManagerList/").success(function (response) {
                $scope.managers = response;
            });
        }
    ]);

})();
