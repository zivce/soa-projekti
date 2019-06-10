const config = require(`./config-other.json`);
const { DeviceHivePlugin } = require(`devicehive-plugin-core`);
const { MessageBuilder, MessageUtils } = require(`devicehive-proxy-message`);
const fetch = require('node-fetch');
const api = require('./api');
const fs = require('fs');
const path = require('path');

/**
 * Plugin main class
 */
class PluginService extends DeviceHivePlugin {

    /**
     * Before plugin starts hook
     */
    beforeStart() {
        console.log(`Plugin is starting`);
    }

    /**
     * After plugin starts hook
     */
    afterStart() {
        const me = this;

        console.log(`Plugin has been started. Subscribed: ${me.isSubscribed}. Topic: ${me.topic}`);

        me.sendMessage(MessageBuilder.health());
        const minutePassed = 60 * 1000;
        setInterval(() => {
            fetch(api.API_URL)
                .then(res => res.json())
                .then((messages) => {
                    const notificationMsg = MessageBuilder.createNotification(
                        {
                            topic: me.topic,
                            message: JSON.stringify({
                                b: { notification: messages }
                            }),
                            partition: 1,
                            type: "latest"
                        }, 44);
                    service.sendMessage(notificationMsg);
                })
        }, minutePassed);
    }

    /**
     * Message handler
     * @param message
     */
    handleMessage(message) {
        switch (message.type) {
            case MessageUtils.HEALTH_CHECK_TYPE:
                console.log(`NEW HEALTH CHECK TYPE MESSAGE. Payload: ${message.payload}`);
                break;
            case MessageUtils.TOPIC_TYPE:
                console.log(`NEW TOPIC TYPE MESSAGE. Payload: ${message.payload}`);
                break;
            case MessageUtils.PLUGIN_TYPE:
                console.log(`NEW PLUGIN TYPE MESSAGE. Payload: ${message.payload}`);
                break;
            case MessageUtils.NOTIFICATION_TYPE:
                try {
                    if (!message.payload) {
                        break;
                    }
                    const messageBody = JSON.parse(message.payload.message).b;
                    if (messageBody.notification) {
                        console.log(`NEW NOTIFICATION TYPE MESSAGE.`);
                        fs.writeFileSync(path.resolve(__dirname, 'admin1.json'),
                            JSON.stringify(messageBody.notification), { flag: 'a' });
                    } else if (messageBody.command) {
                        if (messageBody.command.isUpdated) {
                            console.log(`NEW COMMAND UPDATE TYPE MESSAGE. Command: ${JSON.stringify(messageBody.command)}`);
                        } else {
                            console.log(`NEW COMMAND TYPE MESSAGE. Command: ${JSON.stringify(messageBody.command)}`);
                        }
                    } else {
                        console.log(`UNKNOWN MESSAGE. Body: ${messageBody}`);
                    }
                } catch (error) {
                    console.log(`UNEXPECTED ERROR`);
                    console.log(error);
                }
                break;
            default:
                console.log(`UNKNOWN MESSAGE TYPE`);
                console.log(message);
                break;
        }
    }

    /**
     * Before plugin stops hook
     */
    beforeStop() {
        console.log(`Plugin will stop soon`);
    }

    /**
     * Plugin error handler
     */
    onError(error) {
        console.warn(`PLUGIN ERROR: ${error}`);
    }
}
const service = new PluginService();
DeviceHivePlugin.start(service, config);
