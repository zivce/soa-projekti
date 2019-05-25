const { latestMessagesEndpoint } = require('../config/message-api.config');
const fetch = require('node-fetch');

const plugin = 'message-collect'
const collectMessagesPattern = {
    role: plugin,
    cmd: "collect",
};

function collectMessagesPlugin(options) {
    const seneca = this;
    seneca.add(collectMessagesPattern, (msg, res) => {
        fetch(latestMessagesEndpoint)
            .then(resp => resp.json())
            .then(response => res(null, response));
    });
}
module.exports = {
    collectMessagesPattern,
    collectMessagesPlugin
}