(function () {
    "use strict";

    angular
        .module("hrmsPublicHoliday")
        .controller("PublicHolidayController",
                ["$scope", "$http", "publicHolidayResource", PublicHolidayController]);



    function PublicHolidayController($scope, $http, publicHolidayResource) {
        var vm = this;
        var api = "/api/PublicHoliday/";

        var initilizeController = function () {
            vm.loading = true;
            vm.isWeekend = false;
            vm.publicHolidays = [];
            vm.newHoliday = {};

            getVietnamesePublicHoliday();
            getYears();
        }

        // Watch currentYear
        $scope.$watch('vm.currentYear', function (nVal) {
            getPublicHoliday(nVal);
        });

        $scope.$watch('vm.newHoliday.date', function (nVal) {
            if (nVal !== undefined && nVal !== null) {
                if (nVal.getDay() === 0 || nVal.getDay() === 6) {
                    vm.isWeekend = true;
                    vm.dayInLieu = findFirstWorkingDate(nVal);
                } else {
                    vm.isWeekend = false;
                }
            } else {
                vm.isWeekend = false;
            }
        });

        vm.changeCurrentYear = function () {
            vm.newHoliday.vietnameseHoliday = vm.vietnameseHolidays[0];
            vm.newHoliday.date = new Date(vm.currentYear, 0, 1);
        }

        vm.changeVietnamesHoliday = function () {
            switch (vm.newHoliday.vietnameseHoliday.Name) {
                case "New Year":
                    vm.newHoliday.date = new Date(vm.currentYear, 0, 1);
                    break;
                case "Liberation's day":
                    vm.newHoliday.date = new Date(vm.currentYear, 3, 30);
                    break;
                case "International Workers Day":
                    vm.newHoliday.date = new Date(vm.currentYear, 4, 1);
                    break;
                case "Independence Day":
                    vm.newHoliday.date = new Date(vm.currentYear, 8, 2);
                    break;
                default:
                    vm.newHoliday.date = null;
            }
        }

        vm.openDatepicker = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            vm.opened = !vm.opened;
        }

        vm.openDatepicker2 = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            vm.opened2 = !vm.opened2;
        }

        vm.createHoliday = function (isValid) {
            if (!isValid) {
                alert("Please correct the validation errors first.");
                return;
            }

            var data = {
                VietnamesePublicHolidayId: vm.newHoliday.vietnameseHoliday.Id,
                Date: vm.newHoliday.date.toLocaleDateString()
            };

            $http({
                method: 'POST',
                url: api,
                data: data
            }).
            success(function (data, status, headers, config) {
                toastr.success("Success");
                getPublicHoliday(vm.currentYear);
            }).
            error(function (data, status, headers, config) {
                toastr.fail("Fail");
            });
        }

        vm.deleteHoliday = function (id) {
            $http({
                method: 'DELETE',
                url: api + id
            }).
            success(function (data, status, headers, config) {
                for (var i = 0; i < vm.publicHolidays.length; i++) {
                    if (vm.publicHolidays[i].Id === id) {
                        vm.publicHolidays.splice(i, 1);
                        break;
                    }
                }
                toastr.success("Deleted");
            }).
            error(function (data, status, headers, config) {
                toastr.fail("Fail");
            });


        }

        vm.updateHoliday = function (data, id) {
            console.log(typeof data);
            var date = new Date(data);
            var data2 = {
                Id:id,
                Date: date.toLocaleDateString()
            };
            $http({
                method: 'PUT',
                url: api + id,
                data: data2
            }).
            success(function (data, status, headers, config) {
                toastr.success("Updated");
                //getPublicHoliday(vm.currentYear);
            }).
            error(function (data, status, headers, config) {
                toastr.error("Update fail");
            });

        };
     
        var getPublicHoliday = function (year) {
            vm.loading = true;
            $http.get(api + "/GetByYear/" + year)
                    .success(function (response) {
                        vm.publicHolidays = response;
                        vm.loading = false;
                        
                    });
        }

        var getVietnamesePublicHoliday = function () {
            $http.get("/api/PublicHoliday/GetVietnameseHolidays")
             .success(function (response) {
                 vm.vietnameseHolidays = response;
                 vm.newHoliday.vietnameseHoliday = vm.vietnameseHolidays[0];
                 vm.newHoliday.date = new Date(vm.currentYear, 0, 1);
             });
        };

        var getYears = function () {
            var today = new Date();
            vm.currentYear = today.getFullYear();         
            var startYear = 2015;
            var endYear = vm.currentYear + 1;
            vm.years = [];
            for (var year = startYear; year <= endYear; year++) {
                vm.years.push(year);
            }
        };

        var findFirstWorkingDate = function (date) {
            var newDate = new Date(date);
            if (date.getDay() === 0) {//sunday             
                newDate.setDate(date.getDate() + 1);
            }
            if (date.getDay() === 6) {//saturday
                newDate.setDate(date.getDate() + 2);
            }

            return newDate;
        }

        var init = function () {
            initilizeController();
        };

        init();
    }

})();