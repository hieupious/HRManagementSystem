(function () {

    var controllers = angular.module("hrmsPendingApprovalController", []);

    controllers.controller("PendingApprovalController", ["$scope", "$http",
        function ($scope, $http) {

            $http.get("/api/Users/GetPendingApproval").success(function (response) {
                $scope.records = response;
            });
                        
            $scope.ApproveRequest = function (record) {
                if (record.ApproverComment == null || record.ApproverComment == "") {
                    alert('Please enter your comment.');
                    return;
                }
                if (record.Approved == null) {
                    alert('Please select your choice.');
                    return;
                }
                record.editMode = false;
                $http.put("/api/Users/Approval", record);
            }
        }
    ]);

})();
