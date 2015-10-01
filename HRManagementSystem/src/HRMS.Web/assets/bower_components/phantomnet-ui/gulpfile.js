var gulp = require("gulp"),
    rimraf = require("rimraf"),
    less = require("gulp-less");

var paths = {
    less_bootstrap_lib: "./bower_components/bootstrap/less/variables.less",

    less_bootstrap_local: "./src/less/bootstrap",
    
    less_src: "./src/less/phantomnet.less",
    fonts_src: [
    ],
    js_src: [
        "./src/js/*.js"
    ],

    dist: "./dist",
    css_dist: "./dist/css",
    fonts_dist: "./dist/fonts",
    js_dist: "./dist/js"
};

gulp.task("clean:local", function (cb) {
    rimraf(paths.less_bootstrap_local, cb);
});

gulp.task("clean:dist", function (cb) {
    rimraf(paths.dist, cb);
});

gulp.task("clean", ["clean:dist", "clean:local"]);

gulp.task("consolidate:less:bootstrap", function () {
    gulp.src(paths.less_bootstrap_lib)
        .pipe(gulp.dest(paths.less_bootstrap_local));
});

gulp.task("consolidate:less", ["consolidate:less:bootstrap"]);
gulp.task("consolidate", ["consolidate:less"]);

gulp.task("dist:less", function () {
    gulp.src(paths.less_src)
        .pipe(less({
            relativeUrls: true
        }))
        .pipe(gulp.dest(paths.css_dist));
});

gulp.task("dist:fonts", function () {
    gulp.src(paths.fonts_src)
        .pipe(gulp.dest(paths.fonts_dist));
});

gulp.task("dist:js", function () {
    gulp.src(paths.js_src)
        .pipe(gulp.dest(paths.js_dist));
});

gulp.task("dist", ["dist:less", "dist:fonts", "dist:js"]);

gulp.task("default", ["dist"]);
