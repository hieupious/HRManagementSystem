(function () {

    var controllers = angular.module("hrmsUserSearchControllers", ["hrmsUserSearchServices"]);

    controllers.controller("UserSearchController", ["$scope", "$element", "UserResource",
        function ($scope, $element, UserResource) {
            $scope.users = UserResource.query();
            $scope.searchTerms = null;
        }
    ]);

})();
