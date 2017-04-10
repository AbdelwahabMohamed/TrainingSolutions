var gulp = require('gulp');
var args = require('yargs').argv;
var config = require('./gulp.config')();
var wiredep = require('wiredep').stream;
var del = require('del');
var browserSync = require('browser-sync');
var $ = require('gulp-load-plugins')({
    lazy: true
});
var port = process.env.PORT || config.defaultPort;

//Vetting all js source files against coding errors and style errors
gulp.task('vet', function () {
    log('vetting the code with JSHint and JSCS');
    return gulp
        .src(config.alljs)
        .pipe($.if(args.verbose, $.print()))
        .pipe($.jscs())
        .pipe($.jshint())
        .pipe($.jshint.reporter('jshint-stylish', {
            verbose: true
        }))
        .pipe($.jshint.reporter('fail'));
});

//Compiling less files
gulp.task('styles', ['clean-styles'], function () {
    log('compiling less -> css');
    return gulp
        .src(config.less)
        .pipe($.less())
        .pipe($.plumber())
        .pipe($.autoprefixer({
            browsers: ['last 2 version', '> 5%']
        }))
        .pipe(gulp.dest(config.temp));
});

//removing old css files, the styles task are will run that before compiling a new version of the css
gulp.task('clean-styles', function (done) {
    var files = config.temp + '**/*.css';
    clean(files, done);
});

//injecting references into index.html for bower css js
gulp.task('wiredep', function () {
    log('wire up the bower css js and our app js into html');
    var options = config.getWiredepDefaultOptions();
    return gulp
        .src(config.index)
        .pipe(wiredep(options))
        .pipe($.inject(gulp.src(config.js))) //custom injection for our js files
        .pipe(gulp.dest(config.client));
});

//injecting our custom css alond with other dependencies, less efficient
gulp.task('inject', ['wiredep', 'styles'], function () { //[dependencies] will run in parrallel
    log('wire up the app css into html, and call wiredep');
    return gulp
        .src(config.index)
        .pipe($.inject(gulp.src(config.css)))
        .pipe(gulp.dest(config.client));
});

gulp.task('serv-dev', ['inject'], function () {
    var isDev = true;

    var nodeOptions = {
        script: config.nodeServer,
        delayTime: 1,
        env: {
            'PORT': port,
            'NODE_ENV': isDev ? 'dev' : 'build'
        },
        watch: [config.server]
    };
    return $.nodemon(nodeOptions)
        .on('restart', ['vet'], function (ev) {
            log('nodemon restarted');
            log('file changes:\n: ' + ev);
            //sync browser after server restart, with a delay for the server to be able to restart itself
            setTimeout(function () {
                browserSync.notify('reloading');
                browserSync.reload({
                    stream: false
                });
            }, config.browserReloadDelay);
        })
        .on('start', function () {
            log('nodemon started');
            startBrowserSync();
        })
        .on('crash', function () {
            log('nodemon crached');
        })
        .on('exit', function () {
            log('nodemon exited');
        });
});
////////////////////////////

function startBrowserSync() {
    if (args.noSync || browserSync.active) {
        return;
    }
    log('starting browser sync on port ' + port);

    //watches changes in the css files to re-run the styles task after changes
    gulp.watch([config.less], ['styles'])
        .on('change', function (event) {
            changeEvent(event);
        });

    var options = {
        proxy: 'localhost:' + port,
        port: 3000,
        files: [
            config.client + '**/*.*',
            '!' + config.less,
            config.temp + '**/*.css'
        ],
        ghostMode: {
            clicks: true,
            forms: true,
            scroll: true
        },
        injectChanges: true, //if you can, serve minimum files to the broswer
        logFilechanges: true,
        logLevel: 'debug',
        logPrefix: 'gulp-patterns',
        notify: true,
        reloadDelay: 1000
    };
    browserSync(options);
}

function log(msg) {
    if (typeof (msg) === 'object') {
        for (var item in msg) {
            if (msg.hasOwnProperty(item)) {
                $.util.log($.util.colors.blue(msg[item]));
            }
        }
    } else {
        $.util.log($.util.colors.blue(msg));
    }
}

//TODO: fix bug,  call done even if there are no files to delete
function clean(path, done) {
    del(path).then(function () {
        log('cleaning: ' + $.util.colors.blue(path));
    }).catch(function () {
        log('not cleaning: ' + $.util.colors.blue(path));
    }).then(function () {
        done();
    });
}

function changeEvent(event) {
    var srcPattern = new RegExp('/.*(?=/' + config.source + ')/');
    log('file ' + event.path.replace(srcPattern, '') + ' ' + event.type);
}
