// @ts-check
// TODO fix import not working for some reason
// const { MONGO_DB_CLOUD_URI } = require('../config/db.config');
const mongoose = require("mongoose");
const fetch = require("node-fetch");
const MONGO_DB_CLOUD_URI =
  "mongodb+srv://collector_node:collector_node@socialevolutioncluster-kde4s.mongodb.net/socialEvolutionDb?retryWrites=true";

const MessageEventModel = require("../models/message-event.model");
const EventPublisherEndpoint = "http://172.17.0.1:34567/publish";

const plugin = "message-collect";
const recordPattern = {
  role: plugin,
  cmd: "record"
};
const postEventPattern = {
  role: plugin,
  cmd: "POST",
  subtask: "insertone",
  private: true
};

function recordEvents(options) {
  const seneca = this;
  seneca.add(postEventPattern, (msg, res) => {
    const CurrentPrediction = msg.currentprediction;
    const data = { CurrentPrediction, LastUpdatedAt: new Date() };
    const newEvent = new MessageEventModel(data);
    newEvent.save((err, event) => {
      console.log(event);
      if (err) {
        console.log("error!");
        res(err, null);
        return;
      }
      // fetch post to the mqttpublisher
      // fetch(data)
      checkDataAndAlert({msg: "ipak radi"});
      res(null, event._id);
    });
  });

  seneca.add(recordPattern, (msg, res) => {
    const currentprediction = msg.predicted;
    mongoose.connect(MONGO_DB_CLOUD_URI, { useNewUrlParser: true });
    const db = mongoose.connection;
    db.on("error", () => {
      res("connection error", null);
    });
    db.once("open", () => {
      seneca.act({ ...postEventPattern, currentprediction }, res);
    });
  });
}
module.exports = {
  recordPattern,
  recordEvents
};

function checkDataAndAlert(analyticsData) {
  // alert logic with analyticsData
  const shouldPublishAlert = true;

  if (shouldPublishAlert) {
    const alertMsg = {
      msgType: "messagesServerStackOverflow"
    };
    console.log("Alerting with mqtt")
    fetch(EventPublisherEndpoint, {
      method: "POST",
      body: JSON.stringify(alertMsg),
      headers: { "Content-Type": "application/json" }
    })
      .then(res => res.json())
      .then(json => console.log(json));
  }
}
