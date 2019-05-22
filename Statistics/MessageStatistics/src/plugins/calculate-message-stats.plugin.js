// @ts-check

const calcMessageStatsPattern = {
  role: "statistics",
  cmd: "calculate",
  type: "messages"
};

function messageStatsPlugin(options) {
  this.add(calcMessageStatsPattern, (msg, res) => {
    res({callStats: "Bussiness Logic goes here"})
  });  
}

module.exports = {
  messageStatsPlugin,
  calcMessageStatsPattern
};