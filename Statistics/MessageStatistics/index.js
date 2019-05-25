var spawn = require('child_process').spawn

const services = ['message-processing.service.js', 'restful-message-events.service.js']
services.forEach((service) => {
  spawn('node', ['./src/services/' + service, '--seneca.log.all']);
})