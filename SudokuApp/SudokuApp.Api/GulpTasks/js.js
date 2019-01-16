var plugins = require('gulp-load-plugins')();
module.exports = function (gulp, paths) {
    return function () {
        var stream = gulp.src(['./Controllers/Mvc/*/*/*.js'])
            .pipe(gulp.dest(paths.js))
            .pipe(plugins.uglify())
            .pipe(plugins.rename({ extname: '.min.js' }))
            .pipe(gulp.dest(paths.js));

        return stream;
    };
};