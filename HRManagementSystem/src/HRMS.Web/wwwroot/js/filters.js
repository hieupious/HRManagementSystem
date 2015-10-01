(function () {

    var filters = angular.module("timespanFilter", []);

    filters.filter("timespan", function () {
        return function (input) {
            var hours = Math.floor(input / 60),
                minutes = input % 60;
            return hours + ":" + (minutes < 10 ? "0" : "") + minutes;
        };
    });

    filters.filter("range", function () {
        return function (input, total) {
            total = parseInt(total);

            for (var i = 0; i < total; i++) {
                input.push(i);
            }

            return input;
        };
    });

})();
