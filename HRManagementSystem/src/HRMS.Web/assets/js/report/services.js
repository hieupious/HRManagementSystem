(function () {

    var services = angular.module("hrmsReportServices", ["pnResource"]);

    services.factory("ReportResource", ["Resource",
        function ($resource) {
            return $resource("/api/Reports/:month", { month: '@month' });
        }
    ]);
})();
