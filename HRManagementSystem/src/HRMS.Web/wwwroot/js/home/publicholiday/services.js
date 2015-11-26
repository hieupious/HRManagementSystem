(function () {
    "use strict";

    var service = angular.module("hrmsPublicHolidayService"
                            , ["pnResource"]);
  
    service.factory("publicHolidayResource",
                ["$resource", publicHolidayResource]);

    function publicHolidayResource($resource) {
        return $resource("/api/PublicHoliday/:id", { year: '@year' });
    }
})();