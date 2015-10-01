(function () {

    var DEFAULT_EVENT_DOMAIN = ".uiBootstrapDatetimepicker";

    var directives = angular.module("uiBootstrapDatetimepicker", []);

    directives.directive("uiBootstrapDatetimepicker", function () {
        return {
            restrict: "E",
            replace: true,
            template: '<div class="input-group date"><input type="text" class="form-control" /><span class="input-group-addon"><span></span></span></div>',
            require: "ngModel",
            link: function (scope, element, attrs, ctrl) {
                var group = $(element),
                    input = $("input", element),
                    groupAddon = $(".input-group-addon", element),
                    icon = $("span", groupAddon),
                    options = {};
                $.each(attrs, function (name, value) {
                    if (name.indexOf("grp") === 0) {
                        name = getPropName(name, 3);
                        setProp(group, name, value);
                    } else if (name.indexOf("inp") === 0) {
                        name = getPropName(name, 3);
                        setProp(input, name, value);
                    } else if (name.indexOf("aon") === 0) {
                        name = getPropName(name, 3);
                        setProp(groupAddon, name, value);
                    } else if (name.indexOf("ico") === 0) {
                        name = getPropName(name, 3);
                        setProp(icon, name, value);
                    } else if (name.indexOf("opt") === 0) {
                        name = getPropName(name, 3);
                        options[name] = eval(value);
                    }
                });

                function getPropName(name, prefixLength) {
                    name = name.substr(prefixLength);
                    return name.substr(0, 1).toLowerCase() + name.substr(1);
                }

                function setProp(item, name, value) {
                    if (name === "class") {
                        item.addClass(value);
                    } else {
                        item.attr(name, value);
                    }
                }

                var picker = $(element)
                    .datetimepicker(options)
                    .data("DateTimePicker");

                ctrl.$formatters.push(function (modelValue) {
                    return modelValue;
                });

                ctrl.$parsers.push(function (viewValue) {
                    return typeof(viewValue) === "string" ? new Date(viewValue) : viewValue;
                });

                $(element).on("dp.change" + DEFAULT_EVENT_DOMAIN, function () {
                    var date = picker.date();
                    ctrl.$setViewValue(date ? date.toDate() : null);
                });

                ctrl.$render = function () {
                    if (ctrl.$viewValue === undefined) {
                        picker.clear();
                        return;
                    }
                    picker.date(ctrl.$viewValue);
                };
            }
        };
    });

})();
