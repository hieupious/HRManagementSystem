(function () {

    var services = angular.module("hrmsUserSearchServices", ["pnResource"]);

    services.factory("UserResource", ["Resource",
        function ($resource) {
            return $resource("api/users/:userId", { userId: "@id" });
        }
    ]);

})();
