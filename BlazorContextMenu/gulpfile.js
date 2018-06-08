var gulp = require("gulp"),
    cssmin = require("gulp-cssmin"),
    rename = require("gulp-rename"),
    uglify = require("gulp-uglify"),
    runSequence = require("run-sequence"),
    rimraf = require("rimraf");


gulp.task("clean:js", function (cb) {
    rimraf("content/**/*.min.js", cb);
});

gulp.task("clean:css", function (cb) {
    rimraf("content/**/*.min.css", cb);
});

gulp.task("min:css", function () {
    return gulp.src(["content/**/*.css", "!content/**/*.css.js"], { base: "." })
        .pipe(cssmin())
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(gulp.dest("."));
});

gulp.task("min:js", function () {
    return gulp.src(["content/**/*.js", "!content/**/*.min.js"], { base: "." })
        .pipe(uglify())
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(gulp.dest("."));
});

gulp.task("clean", ["clean:js", "clean:css"]);
gulp.task("min", ["min:css", "min:js"]);

gulp.task("all", function (done) {
    runSequence("clean", "min", function () {
        done();
    });
}); 