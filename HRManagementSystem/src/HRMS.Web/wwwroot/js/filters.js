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

    filters.filter("sum", function () {
        return function (records, property) {
            var sum = 0;
            for(var i = 0; i < records.length; i++) {
                sum = sum + records[i][property];
            }
            return sum;
        }
    });
    
    filters.filter("monthInYear", function () {
        return function (input, year) {
            var day = new Date();
            var month = 12;
            if (year == day.getFullYear()) {
                month = day.getMonth() + 1;
            }
            for (var i = 0; i < month; i++) {
                input.push(i)
            }
            return input;
        }
    });

})();
