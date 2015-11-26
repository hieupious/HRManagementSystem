(function () {
    "use strict";

    var app = angular.module("hrmsPublicHoliday",
            ["ui.bootstrap",
             "xeditable",
             "angular-confirm",
             "hrmsPublicHolidayService"]);

    app.run(function (editableOptions) {
        editableOptions.theme = 'bs3';
    });
})();