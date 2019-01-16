var plugins = require('gulp-load-plugins')();
module.exports = function (gulp, paths) {
    return function () {
        var stream = gulp.src([
            // подгрузка инициализаторов из текущего проекта, те, что не попали в npm пакет
            './Shared/Scripts/Initializers/*.{js}',
            paths.common + 'Shared/Scripts/Utils.js',
            paths.common + 'Shared/Scripts/MessageShower.js',
            paths.common + 'Shared/Scripts/Initializers/*.{js}',
            paths.nodeModules + 'notifyjs-browser/dist/notify.js',
            paths.nodeModules + 'jquery-form/src/jquery.form.js',
            paths.nodeModules + 'jquery-maxlength/jquery.plugin.js',
            paths.nodeModules + 'jquery-maxlength/jquery.maxlength.js'])
            .pipe(plugins.concat('Utils.js'))
            .pipe(gulp.dest(paths.js))
            .pipe(plugins.uglify())
            .pipe(plugins.rename({ extname: '.min.js' }))
            .pipe(gulp.dest(paths.js));

        return stream;
    };
};
