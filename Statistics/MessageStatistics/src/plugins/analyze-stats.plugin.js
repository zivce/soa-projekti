const LinearRegression = require('ml-regression-simple-linear');

const plugin = 'analyze-stats'

const analyzeStatsPattern = {
    role: plugin,
    cmd: "analyze"
};

const buildModelPattern = {
    role: plugin,
    cmd: "analyze",
    subtask: "build-model",
    private: true
};

const predictPattern = {
    role: plugin,
    cmd: "analyze",
    subtask: "predict",
    private: true
};


function analyzeStatsPlugin(options) {
    const seneca = this;

    seneca.add(buildModelPattern, (incomingMessage, res) => {
        const tuples = incomingMessage.tuples;
        const hours = Object.keys(tuples).map(elem => new Number(elem));
        const numberOfMessages = Object.values(tuples).map(elem => new Number(elem));
        const model = new LinearRegression(hours, numberOfMessages);
        res(null, model);
    })

    seneca.add(predictPattern, (incomingMessage, res) => {
        const model = incomingMessage.model;
        const maxHours = 24;
        const arr = new Array(maxHours);
        let predicted = {};
        for (let hour = 0; hour < 24; hour++) {
            predicted[hour] = Math.round(model.predict(hour));
        }
        res(null, predicted);
    })

    // @Public
    seneca.add(analyzeStatsPattern, (incomingMessage, res) => {
        let tuples = incomingMessage.tuples;
        const statsPredicted = (error, predictedData) => {
            if (error) {
                console.log("error!");
                console.error(error);
                return res(error, null);
            }
            res(null, predictedData);
        }

        const modelBuilt = (error, model) => {
            if (error) {
                console.log("error!");
                console.error(error);
                return res(error, null);
            }
            seneca.act({ ...predictPattern, model }, statsPredicted);
        }
        seneca.act({ ...buildModelPattern, tuples }, modelBuilt);
    });
}
module.exports = {
    analyzeStatsPattern,
    analyzeStatsPlugin
}