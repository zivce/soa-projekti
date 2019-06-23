// This service should start the process of fetching and parsing
// messages into events

const { collectMessagesPattern, collectMessagesPlugin } = require('../plugins/message-collector.plugin');
const { processMessagesPattern, processMessagesPlugin } = require('../plugins/process-messages.plugin')
const { analyzeStatsPattern, analyzeStatsPlugin } = require('../plugins/analyze-stats.plugin')
const { recordEvents, recordPattern } = require('../plugins/record-events.plugin');
require('seneca')()
    .use(collectMessagesPlugin)
    .use(processMessagesPlugin)
    .use(analyzeStatsPlugin)
    .use(recordEvents)
    .ready(function () {

        const serviceFinished = (error, eventid) => {
            if (error) {
                console.log("error!");
                console.error(error);
                return;
            }
            console.log(`Added event with id ${eventid} successfully`);
        }


        const messagesAnalyzed = (error, predicted) => {
            if (error) {
                console.log("error!");
                console.error(error);
                return;
            }

            this.act({ ...recordPattern, predicted }, serviceFinished)
        }
        const messagesProcessed = (error, tuples) => {
            if (error) {
                console.log("error!");
                console.error(error);
                return;
            }
            this.act({ ...analyzeStatsPattern, tuples }, messagesAnalyzed);
        }

        const messagesFetched = (error, messages) => {
            if (error) {
                console.log("error!");
                console.error(error);
                return;
            }
            console.log('Pre set interval u msg stats. Messages fetched')
            setInterval(() => {
                this.act({ ...processMessagesPattern, messages: messages }, messagesProcessed);
            }, 5000);
        }
        this.act(collectMessagesPattern, messagesFetched);
    })
