// @ts-check

const server = require("./src/app");
const serverConfig = require("./src/config/server.config");

server.listen(serverConfig.port, () => {
  const msg = `Server '${serverConfig.name}' started successfully...`;
  console.log(msg);
});
