// @ts-check

const senecaConstructor = require("seneca");
const server = require("express")();
const bodyParser = require("body-parser");
const serverConfig = require("./config/server.config");
const { calcMessageStatsPattern, messageStatsPlugin } = require("./plugins/calculate-message-stats.plugin");

const senecaPlugins = [messageStatsPlugin];
const seneca = getSenecaWithPlugins(senecaPlugins);

server.use(bodyParser.json());

seneca.listen();

// seneca.act({ ...calcMessageStatsPattern, messages: ["prva", "druga"] }, console.log);

server.get("/", (req, res, next) => {
  const debugDescription = `This is the endpoint for '${serverConfig.name}' service.`;
  res.json({ debugDescription });
});






function getSenecaWithPlugins(plugins, senecaInstance = senecaConstructor()) {
  plugins.forEach(plugin => {
    senecaInstance.use(plugin);
  });
  return senecaInstance;
};

module.exports = server;
