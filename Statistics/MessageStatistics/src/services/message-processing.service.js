// This service should start the process of fetching and parsing
// messages into events
const { collectMessagesPattern } = require("../plugins/message-collector.plugin");
const { calcMessageStatsPattern } = require("../plugins/process-messages.plugin.js");

require('seneca')()
    .use('../plugins/message-collector.plugin.js')
    .use('../plugins/process-messages.plugin.js')
    .ready(function () {
        this.act(collectMessagesPattern, messagesFetched);
        const messagesFetched = (error, messages) => {
            if (error) {
                console.error(error);
                return;
            }
            this.act({ ...calcMessageStatsPattern, messages: messages }, console.log);
        }
    })
