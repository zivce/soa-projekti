const plugin = 'message-collector';

const latestEventsPattern = {
    role: plugin,
    cmd: "latest-events",
};

const eventsPattern = {
    role: plugin,
    cmd: "all-events",
};

function collectEvents() {
    const seneca = this;

    seneca.add(latestEventsPattern, fetchLatestHandler);
    const fetchLatestHandler = () => {
    }


    seneca.add(eventsPattern, fetchAllHandler);
    const fetchAllHandler = () => {
    }

    return { name: plugin };
}


module.exports = {
    eventsPattern,
    latestEventsPattern,
    collectEvents
}