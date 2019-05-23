var spawn = require('child_process').spawn

const services = ['message-processing.service.js', 'rest-message-events.service.js']
services.forEach((service) => {
  spawn('node', ['./src/services/' + service], '--seneca.log.all');
})