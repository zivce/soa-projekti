const Express = require("express");
const BodyParser = require("body-parser");
const seneca = require('seneca')();
const SenecaWeb = require('seneca-web');
const expressSettings = require('../config/server.config');
const { api } = require('../plugins/api.plugin');

const routes = [{
  prefix: '/api/',
  pin: 'role:api,cmd:*',
  map: {
    'latest-events': { GET: true },
    'all-events': { GET: true }
  }
}];

var app = Express()
app.use(BodyParser.urlencoded({ extended: true }))

const serverConfig = {
  routes: routes,
  context: app,
  adapter: require('seneca-web-adapter-express')
}

seneca
  .use(api)
  .use(SenecaWeb, serverConfig)
  .ready(() => {
    const app = seneca.export('web/context')()
    app.listen(expressSettings.port, (err) => {
      console.log(err || 'server started on:' + expressSettings.port)
    })
  })

