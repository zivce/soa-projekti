// @ts-check

const mongoose = require("mongoose");
const Schema = mongoose.Schema;

const MessageSchemaDefinition = {
    originUserId: String,
    timestamp: Date,
    isIncomping: Boolean,
    destUserId: String,
    destPhoneHash: String
}

const MessageSchema = new Schema(MessageSchemaDefinition);

module.exports = mongoose.model("Message", MessageSchema);
