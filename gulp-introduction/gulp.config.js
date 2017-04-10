module.exports = function () {
    var client = './src/client/';
    var clientApp = client + 'app/';
    var server = './src/server/';
    var temp = './.tmp/';

    var config = {
        /*File paths*/
        //all js to be vet
        alljs: ['./src/**/*.js',
            './*.js'
        ],
        less: client + 'styles/styles.less',
        index: client + 'index.html',
        js: [
            clientApp + '**/*.module.js',
            clientApp + '**/*.js',
            '!' + clientApp + '**/*.spec.js'
        ],
        css: temp + 'styles.css',
        client: client,
        temp: temp,
        server: server,
        /*bower and npm locations */
        bower: {
            json: require('./bower.json'),
            directory: './bower_components',
            ignorePath: '../..'
        },
        /*node settings*/
        defaultPort: 7203,
        nodeServer: server + 'app.js',
        browserReloadDelay: 1000
    };
    config.getWiredepDefaultOptions = function () {
        var options = {
            bowerJson: config.bower.json,
            directory: config.bower.directory,
            ignorePath: config.bower.ignorePath
        };
        return options;
    };

    return config;
};
