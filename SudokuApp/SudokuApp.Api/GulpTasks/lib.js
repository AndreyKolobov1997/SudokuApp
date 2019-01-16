var plugins = require('gulp-load-plugins')();
module.exports = function (gulp, paths) {
    return function () {

        var stream = gulp.src([
                './Shared/Scripts/lib/*.js',
                paths.nodeModules + 'jquery/dist/jquery.js',
                paths.nodeModules + 'jquery-validation/dist/jquery.validate.js',
                paths.nodeModules + 'jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js',
                paths.nodeModules + 'jquery-ui-dist/jquery-ui.js'
            ])
            .pipe(gulp.dest(paths.libraries))
            .pipe(plugins.uglify())
            .pipe(plugins.rename({ extname: '.min.js' }))
            .pipe(gulp.dest(paths.libraries));

        return stream;
    };
};
