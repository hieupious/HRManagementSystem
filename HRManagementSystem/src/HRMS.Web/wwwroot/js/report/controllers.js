(function () {

    var controllers = angular.module("hrmsReportControllers", ["hrmsReportServices"]);

    controllers.controller("ReportController", ["$scope", "$filter", "ReportResource",
        function ($scope, $filter, ReportResource) {
            var today = new Date();

            $scope.startYear = 2013;
            $scope.endYear = today.getFullYear();
            $scope.year = today.getFullYear().toString();
            $scope.month = (today.getMonth() + 1).toString();
            $scope.date = null;
            $scope.users = [];
            $scope.loading = false;
            $scope.refreshUsers = function () {
                if ($scope.year === "" || $scope.month === "") {
                    $scope.date = null;
                    $scope.users = [];
                } else {
                    $scope.loading = true;
                    $scope.date = new Date($scope.year, $scope.month - 1, 1);
                    var month = $filter('date')($scope.date, 'yyyy-MM-dd');
                    ReportResource.query({ month: month }, function (records) {
                        $scope.users = records;
                        $scope.loading = false;
                    });
                }
                $.each($scope.users, function (userIndex, user) {
                    user.totalLackTime = 0;
                    $.each(user.Dates, function (dateIndex, date) {
                        user.totalLackTime += date.LackTime;
                    });
                });
            };

            $scope.refreshUsers();
                       
        }
    ]);

})();


var GG = {
   tableToExcel : function (table, name) {
        /// <signature>
        /// <param name='table' type='jQuery' />
        /// <param name='name' type='String' />
        /// <returns type='String'/>
        /// </signature>
        /*
         * Source :http://www.codeproject.com/Tips/755203/Export-HTML-table-to-Excel-With-CSS
         * by Er. Puneet Goel
         */

        var uri = "data:application/vnd.ms-excel;base64,",
            template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>';

        var ctx = {
            worksheet: name || "Worksheet",
            table: table.html()
        };
        return uri + base64(format(template, ctx));

        function base64(s) {
            return window.btoa(unescape(encodeURIComponent(s)));
        }

        function format(s, c) {
            return s.replace(/{(\w+)}/g, function (m, p) {
                return c[p];
            });
        }
   },

   setUpExport: function () {
       var table = $("table"),
       original = table.html();
       table = $("<table></table>").html(original);
       $(".excel-hidden", table).remove();

       $("th, td", table).css({
           "vertical-align": "top",
           "border-top": ".5pt solid windowtext",
           "border-right": ".5pt solid windowtext",
           "border-bottom": ".5pt solid windowtext",
           "border-left": ".5pt solid windowtext"
       });
       $("th", table).not(".text-right").not(".text-center").css("text-align", "left");
       $("th.text-right", table).css("text-align", "right");

       $("a", table).replaceWith(function () {
           return $(this).text();
       });

       $("li", table).replaceWith(function () {
           return "<div>" + $(this).text() + "</div>";
       });
       $("ul", table).replaceWith(function () {
           return $(this).html();
       });

       $("td[data-date-value]", table).each(function (index, element) {
           var $element = $(element);
           $element.html($element.data("date-value"));
           $element.attr("style", "mso-number-format:'dd/mm/yyyy;@'; text-align: center;" + $(element).attr("style"));
       });
       var title = $('.toolbox a').attr("download");
       $("thead", table).prepend($("<tr></tr>").append($("<th></th>").attr("colspan", 13)
                                                                     .css({
                                                                         "text-align": "left",
                                                                         "font-size": "25px",
                                                                         "height": "50px"
                                                                     })
                                                                     .append(title)));
       $("#export").prop("href", GG.tableToExcel(table, "2015"));       
   }
}
