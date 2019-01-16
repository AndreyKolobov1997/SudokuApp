/// <binding ProjectOpened='js, js-utils-and-tiny-plugins, less, lib, watch, svg-sprites, js-shared' />
"use strict";

var gulp = require("gulp");

var paths = {
    serviceName: "sudoku/"
};

paths.webroot = "./wwwroot/" + paths.serviceName + "static/";
paths.js = paths.webroot + "js/";
paths.concatCssDest = paths.webroot + "css/";
paths.libraries = paths.js + "lib/";
paths.nodeModules = "./node_modules/";
paths.tasks = "./GulpTasks/";

paths.jsShared = paths.js + "Shared/";
paths.img = paths.webroot + "img/";

gulp.task("js-shared", require(paths.tasks + 'jsShared')(gulp, paths));

gulp.task("js", require(paths.tasks + 'js')(gulp, paths));

gulp.task("less", require(paths.tasks + 'less')(gulp, paths));

gulp.task("js-utils-and-tiny-plugins", require(paths.tasks + 'utilsAndPlugins')(gulp, paths));

gulp.task("lib", require(paths.tasks + 'lib')(gulp, paths));

gulp.task("images-static", require(paths.tasks + 'commonStatic')(gulp, paths));