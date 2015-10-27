(function () {

    var services = angular.module("hrmsReportServices", ["pnResource"]);

    services.factory("ReportResource", ["Resource",
        function ($resource) {
            return $resource("/api/Users/GetMonthlyWorkingReport?month=:month", {
            month: '@month' });
        }
    ]);
})();
