var gulp = require("gulp"),
    cssmin = require("gulp-cssmin"),
    rename = require("gulp-rename"),
    uglify = require("gulp-uglify"),
    rimraf = require("rimraf");


gulp.task("clean:js", function (cb) {
    rimraf("wwwroot/**/*.min.js", cb);
});

gulp.task("clean:css", function (cb) {
    rimraf("wwwroot/**/*.min.css", cb);
});

gulp.task("min:css", function () {
    return gulp.src(["wwwroot/**/*.css", "!wwwroot/**/*.min.css"], { base: "." })
        .pipe(cssmin())
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(gulp.dest("."));
});

gulp.task("min:js", function () {
    return gulp.src(["wwwroot/**/*.js", "!wwwroot/**/*.min.js"], { base: "." })
        .pipe(uglify())
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(gulp.dest("."));
});

gulp.task("clean", gulp.parallel("clean:js", "clean:css"));
gulp.task("min",  gulp.parallel("min:css", "min:js"));
gulp.task("all", gulp.series("clean", "min")); 