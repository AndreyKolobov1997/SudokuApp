var plugins = require('gulp-load-plugins')();
module.exports = function (gulp, paths) {
    return function () {

        var stream = gulp.src(['!' + './Shared/Scripts/Utils.{js}',
        '!' + './Shared/Scripts/Initializers/*.{js}',
            './Shared/Scripts/*.{js}',
            './Shared/Views/Components/**/*.{js}'])
            .pipe(gulp.dest(paths.jsShared))
            .pipe(plugins.uglify())
            .pipe(plugins.rename({ extname: '.min.js' }))
            .pipe(gulp.dest(paths.jsShared));

        return stream;
    };
};

