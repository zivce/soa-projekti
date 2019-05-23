// @ts-check
const plugin = 'message-process'

const calcMessageStatsPattern = {
  role: plugin,
  cmd: "parse"
};

function messageStatsPlugin(options) {
  const seneca = this;

  seneca.add(calcMessageStatsPattern, (messages, res) => {
    console.log(messages);
  });
}

module.exports = {
  messageStatsPlugin,
  calcMessageStatsPattern
};