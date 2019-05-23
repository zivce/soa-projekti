// @ts-check
const plugin = 'message-collect'
const { latestMessagesEndpoint } = require('../config/message-api.config');
const fetch = require('node-fetch');

const collectMessagesPattern = {
    role: plugin,
    cmd: "collect",
};

function collectMessagesPlugin(options) {
    const seneca = this;

    seneca.add(collectMessagesPattern, async (msg, res) => {
        // @ts-ignore
        const response = await fetch(latestMessagesEndpoint)
        const responseJson = response.json();
        console.log(responseJson);
        res(null, responseJson);
    });
}

module.exports = {
    collectMessagesPlugin,
    collectMessagesPattern
};