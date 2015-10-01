(function () {

    var controllers = angular.module("hrmsUserSearchControllers", ["hrmsUserSearchServices"]);

    controllers.controller("UserSearchController", ["$scope", "UserResource",
        function ($scope, UserResource) {
            $scope.users = UserResource.query();
            $scope.searchTerms = null;
        }
    ]);

})();
