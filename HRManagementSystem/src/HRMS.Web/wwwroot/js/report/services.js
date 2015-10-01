(function () {

    var services = angular.module("hrmsReportServices", []);

    services.factory("ReportResource",
        function () {
            return {
                query: function (month) {
                    return [
                        {
                            Id: 223,
                            Name: "Tran Quoc Linh",
                            Department: {
                                Id: 7,
                                Name: "2XX GP OFFICE"
                            },
                            Dates: [
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
                                    LackTime: 20
                                }
                            ],
                        },
                        {
                            Id: 209,
                            Name: "Nguyen Luong Yen Vy",
                            Department: {
                                Id: 7,
                                Name: "2XX GP OFFICE"
                            },
                            Dates: [
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
                                    LackTime: 20
                                }
                            ],
                        },
                        {
                            Id: 205,
                            Name: "Hoang Trung Hieu",
                            Department: {
                                Id: 7,
                                Name: "2XX GP OFFICE"
                            },
                            Dates: [
                                {
                                    Date: new Date(),
                                    Checkin: new Date(),
                                    Checkout: new Date(),
                                    LackTime: (2 * 60) + 16
                                }
                            ],
                        },
                        {
                            Id: 251,
                            Name: "Nguyen Thao Nguyen",
                            Department: {
                                Id: 7,
                                Name: "2XX GP OFFICE"
                            },
                            Dates: [
                                {
                                    Date: new Date(),
                                    Checkin: new Date(),
                                    Checkout: new Date(),
                                    LackTime: (2 * 60) + 16
                                }
                            ],
                        }
                    ];
                }
            };
        }
    );

})();
