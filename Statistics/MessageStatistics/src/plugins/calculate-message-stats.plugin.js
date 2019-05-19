// @ts-check

const calcMessageStatsPattern = {
  role: "statistics",
  cmd: "calculate",
  type: "messages"
};

function messageStatsPlugin(options) {
  this.add(calcMessageStatsPattern, (msg, res) => {
    res(null, { result: `Porucice xexe: ${msg.messages}` });
  });  
}

module.exports = {
  messageStatsPlugin,
  calcMessageStatsPattern
};