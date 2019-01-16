var plugins = require('gulp-load-plugins')();
module.exports = function (gulp, paths) {
    return function () {

        var stream = gulp.src([paths.common + 'Images/*',
                './Shared/Images/*'])
            .pipe(gulp.dest(paths.img));

        return stream;
    };
};