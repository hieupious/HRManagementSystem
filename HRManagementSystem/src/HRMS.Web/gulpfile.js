/// <binding ProjectOpened='watch' />

var TOOLKIT_THEME = "theme-marketing",
    FONT_NAME = "lato";

var gulp = require("gulp"),
    del = require("del"),
    less = require("gulp-less"),
    sourcemaps = require("gulp-sourcemaps");

var paths = {
    less_toolkit_lib: "./assets/lib/" + TOOLKIT_THEME + "/less/**/*",
    less_phantomnet_lib: "./assets/bower_components/phantomnet-ui/src/less/**/*",

    less_toolkit_local: "./assets/less/toolkit",
    less_phantomnet_local: "./assets/less/phantomnet-ui",

    less_src: "./assets/less/site.less",
    css_src: [
        "./assets/bower_components/font-awesome/css/font-awesome.css"
    ],
    less_watch: "./assets/less/**/*",
    fonts_src: [
        "./assets/lib/theme-marketing/fonts/*",
        "./assets/lib/fonts/" + FONT_NAME + "/*",
        "./assets/bower_components/font-awesome/fonts/*"
    ],
    js_src: [
        "./assets/bower_components/jquery/dist/jquery.js",
        "./assets/bower_components/bootstrap/dist/js/bootstrap.js",
        "./assets/bower_components/angular/angular.js",
        "./assets/bower_components/angular-resource/angular-resource.js",
        "./assets/bower_components/ng-focus-if/focusIf.js",
        "./assets/bower_components/phantomnet-resource/dist/phantomnet-resource.js",
        "./assets/js/**/*"
    ],

    css_dist: "./wwwroot/css",
    fonts_dist: "./wwwroot/fonts",
    js_dist: "./wwwroot/js"
};

paths.dist = [
    paths.css_dist,
    paths.fonts_dist,
    paths.js_dist
];

gulp.task("clean:local", function (cb) {
    del([
        paths.less_toolkit_local,
        paths.less_phantomnet_local
    ], cb);
});

gulp.task("clean:dist", function (cb) {
    del([
        paths.css_dist,
        paths.fonts_dist,
        paths.js_dist
    ], cb);
});

gulp.task("clean", ["clean:dist", "clean:local"]);

gulp.task("consolidate:less:toolkit", function () {
    gulp.src(paths.less_toolkit_lib)
        .pipe(gulp.dest(paths.less_toolkit_local));
});

gulp.task("consolidate:less:phantomnet", function () {
    gulp.src(paths.less_phantomnet_lib)
        .pipe(gulp.dest(paths.less_phantomnet_local));
});

gulp.task("consolidate:less", ["consolidate:less:toolkit", "consolidate:less:phantomnet"]);
gulp.task("consolidate", ["consolidate:less"]);

gulp.task("dist:less", function () {
    gulp.src(paths.less_src)
        .pipe(sourcemaps.init())
        .pipe(less({
            relativeUrls: true
        }))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(paths.css_dist));
});

gulp.task("dist:css", function () {
    gulp.src(paths.css_src)
        .pipe(gulp.dest(paths.css_dist));
});

gulp.task("watch", function () {
    gulp.watch(paths.less_watch, ["dist:less"]);
    gulp.watch(paths.js_src, ["dist:js"]);
});

gulp.task("dist:fonts", function () {
    gulp.src(paths.fonts_src)
        .pipe(gulp.dest(paths.fonts_dist));
});

gulp.task("dist:js", function () {
    gulp.src(paths.js_src)
        .pipe(gulp.dest(paths.js_dist));
});

gulp.task("dist", ["dist:css", "dist:less", "dist:fonts", "dist:js"]);
