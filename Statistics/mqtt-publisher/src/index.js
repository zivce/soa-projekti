// @ts-check

const app = require("express")();
const bodyParser = require("body-parser");
const { initThingsBoardClients, publishToMQTT } = require("./publisher");

const thingsboardHost = "172.17.0.1";
const accessTokens = ["KScw8NnHCX5z4jDhzTPU"];
const expressListenPort = 34567;

app.use(bodyParser.json());

const clients = initThingsBoardClients(thingsboardHost, accessTokens);

app.post("/publish", publishToMQTT(clients));

app.listen(expressListenPort, () => {
    console.log(`MQTT publisher is listening on port ${expressListenPort}`);
});
