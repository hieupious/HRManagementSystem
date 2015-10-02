(function () {

    var services = angular.module("hrmsUserInfoServices", ["pnResource"]);

    services.factory("UserLogResource", ["Resource",
        function ($resource) {
            return $resource("/api/Users/:empId/Report/:month", { empId : '@empId', month : '@month'});
        }
    ]);

})();
