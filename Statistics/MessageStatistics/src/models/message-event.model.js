// @ts-check
const mongoose = require("mongoose");
const Schema = mongoose.Schema;

const MessageEventSchemaDefinition = {
    CurrentPrediction: Object,
    LastUpdatedAt: Object
}

const messageEventSchema = new Schema(MessageEventSchemaDefinition);

const modelName = "Message Event";
const collectionName = "MessageEvents";

module.exports = mongoose.model(modelName, messageEventSchema, collectionName);
