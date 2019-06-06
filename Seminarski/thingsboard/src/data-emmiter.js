// @ts-check

const mqtt = require('mqtt');

// Don't forget to update accessToken constant with your device access token
const thingsboardHost = 'demo.thingsboard.io';
const accessTokens = [ 'PV3CF9bG2saqH8FzQtbe' ];
const minTemperature = 17.5;
const maxTemperature = 30;
const valueOffset = 0.03;
const minHumidity = 12;
const maxHumidity = 90;

const periodInSec = 5;

initEmmiters(accessTokens);

function initEmmiters(accessTokens) {
	accessTokens.forEach((accessToken) => {
		// Initialization of temperature and humidity data with random values
		const initialData = {
			temperature: minTemperature + (maxTemperature - minTemperature) * Math.random(),
			humidity: minHumidity + (maxHumidity - minHumidity) * Math.random()
		};

		// Initialization of mqtt client using Thingsboard host and device access token
		console.log(`Connecting to: ${thingsboardHost} using access token: ${accessToken}`);

		const clientOptions = {
			username: accessToken
		};
		const client = mqtt.connect(`mqtt://${thingsboardHost}`, clientOptions);

		// Triggers when client is successfully connected to the Thingsboard server
		client.on('connect', function() {
			console.log('Client connected!');
			// Uploads firmware version and serial number as device attributes using 'v1/devices/me/attributes' MQTT topic
			client.publish(
				'v1/devices/me/attributes',
				JSON.stringify({
					firmware_version: '1.0.1',
					serial_number: 'SN-001'
				})
			);
			// Schedules telemetry data upload once per second
			console.log(`Uploading temperature and humidity data once per ${periodInSec} seconds...`);
			setInterval(publishTelemetry(client, initialData), periodInSec * 1000);

			//Catches ctrl+c event
			process.on('SIGINT', function() {
				console.log();
				console.log('Disconnecting...');
				client.end();
				console.log('Exited!');
				process.exit(2);
			});

			//Catches uncaught exceptions
			process.on('uncaughtException', function(e) {
				console.log('Uncaught Exception...');
				console.log(e.stack);
				process.exit(99);
			});
		});
	});
}

// Uploads telemetry data using 'v1/devices/me/telemetry' MQTT topic
function publishTelemetry(client, prevData) {
	const data = { ...prevData };
	return () => {
		data.temperature = genNextValue(data.temperature, minTemperature, maxTemperature, valueOffset);
		data.humidity = genNextValue(data.humidity, minHumidity, maxHumidity, valueOffset);
		client.publish('v1/devices/me/telemetry', JSON.stringify(data));
	};
}

// Generates new random value that is within 3% range from previous value
function genNextValue(prevValue, min, max, valueOffset) {
	const calcValue = prevValue + (max - min) * (Math.random() - 0.5) * valueOffset;
	const newValue = Math.max(min, Math.min(max, calcValue));
	return Math.round(newValue * 10) / 10;
}
