(function () {

    var services = angular.module("hrmsUserInfoServices", []);

    services.factory("UserLogResource",
        function () {
            return {
                query: function (month) {
                    return [
                        {
                            Date: new Date(),
                            Checkin: new Date(),
                            Checkout: new Date(),
                            LackTime: (2 * 60) + 16
                        },
                        {
                            Date: new Date(),
                            Checkin: new Date(),
                            Checkout: new Date(),
                            LackTime: 0
                        }
                    ];
                }
            };
        }
    );

})();
