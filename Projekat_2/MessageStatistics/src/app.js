// @ts-check

const seneca = require("seneca")();
const server = require("express")();
const bodyParser = require("body-parser");
const serverConfig = require("./config/server.config");

server.use(bodyParser.json());

const actOnMathSum = (err, result) => {
  if (err) return console.error(err);
  console.log(result);
};

seneca.add("role:math,cmd:sum", (msg, reply) => {
  reply(null, { answer: msg.left + msg.right });
});

seneca.act({ role: "math", cmd: "sum", left: 1, right: 2 }, actOnMathSum);

server.get("/", async (req, res, next) => {
  const debugDescription = `This is the endpoint for '${serverConfig.name}' service.`;
  res.json({ debugDescription });
});

module.exports = server;
