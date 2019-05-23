const serverConfig = require("../config/server.config");

// @ts-check

const server = require("express")();
const bodyParser = require("body-parser");
const seneca = require('seneca')();

// Seneca settings
// Provides REST endpoints to message events data 
seneca
  .use('./plugins/api.plugin.js')
  .client({ port: serverConfig.port, pin: { role: 'latest-events', cmd: '*' } })
  .client({ port: serverConfig.port, pin: { role: 'all-events', cmd: '*' } });

// Express settings
server.use(bodyParser.json());
server.use(bodyParser.urlencoded({ extended: true }));
server.use(seneca.export('web'));
// @ts-check


server.listen(serverConfig.port, () => {
  const msg = `Server '${serverConfig.name}' started successfully...`;
  console.log(msg);
});

