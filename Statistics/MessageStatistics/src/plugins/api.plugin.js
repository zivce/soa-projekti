const mongoose = require('mongoose');
const MONGO_DB_CLOUD_URI = "mongodb+srv://collector_node:collector_node@socialevolutioncluster-kde4s.mongodb.net/socialEvolutionDb?retryWrites=true";
const MessageEventModel = require('../models/message-event.model');

const plugin = 'api';

const latestApiPattern = {
    role: plugin,
    cmd: "latest-events",
};

const allApiPattern = {
    role: plugin,
    cmd: "all-events",
};

function api(options) {
    var seneca = this;


    const latestEventsCommand = (args, done) => {
        console.log("latest events api triggered!");

        mongoose.connect(MONGO_DB_CLOUD_URI, { useNewUrlParser: true });
        db = mongoose.connection;
        db.on('error', () => {
            done('connection error', null);
        })
        db.once('open', () => {
            MessageEventModel.findOne({}, {}, { sort: { 'LastUpdatedAt': -1 } }, function (err, messageevent) {
                if (err) {
                    done(err, null);
                }
                done(null, messageevent)
            });
        })

    }

    seneca.add(latestApiPattern, latestEventsCommand)


    const eventsCommand = (args, done) => {
        console.log("all events api triggered!");

        mongoose.connect(MONGO_DB_CLOUD_URI, { useNewUrlParser: true });
        db = mongoose.connection;
        db.on('error', () => {
            done('connection error', null);
        })
        db.once('open', () => {
            MessageEventModel.find({}, {}, {}, function (err, messageevent) {
                if (err) {
                    done(err, null);
                }
                done(null, messageevent)
            });
        })
    }

    seneca.add(allApiPattern, eventsCommand)

    return { name: plugin };
}
module.exports = {
    api,
    latestApiPattern,
    allApiPattern
};