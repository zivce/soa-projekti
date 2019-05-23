const { eventsPattern, latestEventsPattern } = require("./message-event-collector.plugin");

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

    seneca.add(latestApiPattern, latestEventsCommand)

    const latestEventsCommand = (args, done) => {
        this.act(latestEventsPattern, done)
    }

    seneca.add(allApiPattern, eventsCommand)

    const eventsCommand = (args, done) => {
        this.act(eventsPattern, done)
    }


    seneca.act({
        role: 'web',
        use: {
            prefix: '/api/',
            pin: { role: plugin, cmd: '*' },
            map: {
                'latest-events': { GET: true },
                'all-events': { GET: true }
            }
        }
    })


    return { name: plugin };
}
module.exports = api;