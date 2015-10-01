(function () {

    var services = angular.module("pnResource", ["ngResource"]);

    services.factory("Resource", ["$resource",
        function ($resource) {
            return function (url, params, methods) {
                var defaults = {
                    update: { method: "put", isArray: false },
                    create: { method: "post" }
                };

                methods = angular.extend(defaults, methods);

                var resource = $resource(url, params, methods);

                resource.prototype.$save = function (parameters, success, error) {
                    if (!this.id && !this.Id) {
                        return this.$create(parameters, success, error);
                    }
                    else {
                        return this.$update(parameters, success, error);
                    }
                };

                return resource;
            };
        }
    ]);

})();
