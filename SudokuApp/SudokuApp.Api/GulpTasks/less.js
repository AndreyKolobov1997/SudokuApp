var plugins = require('gulp-load-plugins')();
module.exports = function (gulp, paths) {
    return function () {
        
        var stream = gulp.src([
            './Controllers/Mvc/**/*.less',
            './Shared/**/*.less'])
            .pipe(plugins.plumber())
            .pipe(plugins.less({
                paths: ['./Shared/Styles/*.less']
            }))
            .pipe(plugins.concat('styles.css'))
            .pipe(gulp.dest(paths.concatCssDest))
            .pipe(plugins.rename({ extname: '.min.css' }))
            .pipe(plugins.cleanCss({ compatibility: 'ie8' }))
            .pipe(gulp.dest(paths.concatCssDest));

        return stream;

    };
};