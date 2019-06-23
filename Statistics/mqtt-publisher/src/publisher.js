// @ts-check

const mqtt = require("mqtt");

function initThingsBoardClients(thingsboardHost, accessTokens) {
    const clients = accessTokens.map(accessToken => {
        console.log(`Connecting to: ${thingsboardHost} using access token: ${accessToken}`);

        const clientOptions = {
            username: accessToken
        };

        const client = mqtt.connect(`mqtt://${thingsboardHost}`, clientOptions);
        // @ts-ignore
        client.id = accessToken;
        client.on("connect", function() {
            console.log(`Client ${accessToken} connected!`);
            client.publish(
                "v1/devices/me/attributes",
                JSON.stringify({
                    firmware_version: "1.0.1",
                    serial_number: "SN-001"
                })
            );

            //Catches ctrl+c event
            process.on("SIGINT", function() {
                console.log();
                console.log("Disconnecting...");
                client.end();
                console.log("Exited!");
                process.exit(2);
            });

            //Catches uncaught exceptions
            process.on("uncaughtException", function(e) {
                console.log("Uncaught Exception...");
                console.log(e.stack);
                process.exit(99);
            });
        });
        return client;
    });

    return clients;
}

function publishToMQTT(clients) {
    return (req, res) => {
        const { clientId, ...data } = req.body;
        clients
            // .filter(client => client.id === clientId)
            .forEach(client => {
                console.log("Publishujem sad", data);
                client.publish("v1/devices/me/telemetry", JSON.stringify(data));
            });
        res.send({ msg: "success" });
    };
}

module.exports = {
    initThingsBoardClients,
    publishToMQTT
};
