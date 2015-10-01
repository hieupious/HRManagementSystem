var gulp = require("gulp"),
    rimraf = require("rimraf");

var paths = {
    js_src: [
        "./src/js/*.js"
    ],

    dist: "./dist",
    js_dist: "./dist/js"
};

gulp.task("clean:dist", function (cb) {
    rimraf(paths.dist, cb);
});

gulp.task("clean", ["clean:dist"]);

gulp.task("dist:js", function () {
    gulp.src(paths.js_src)
        .pipe(gulp.dest(paths.js_dist));
});

gulp.task("dist", ["dist:js"]);

gulp.task("default", ["dist"]);
