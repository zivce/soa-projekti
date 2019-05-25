const plugin = 'message-process'
const processMessagesPattern = {
  role: plugin,
  cmd: "parse"
};

const extractMessageStats = {
  role: plugin,
  cmd: "parse",
  subtask: "tuples",
  private: true
};

function processMessagesPlugin(options) {
  const seneca = this;

  seneca.add(extractMessageStats, (incomingMessage, res) => {
    const messages = incomingMessage.messages;
    let tuples = messages.reduce((acc, currMsg) => {
      if (!currMsg.timeStamp) return;

      let hour = currMsg.timeStamp.split("T")[1].split(":")[0];
      let prevNumberOfMessages = acc[hour];
      acc[hour] = !!prevNumberOfMessages ? prevNumberOfMessages + 1 : 1;
      return acc;
    }, {});

    res(null, tuples);
  })

  seneca.add(processMessagesPattern, (incomingMessage, res) => {
    let messages = incomingMessage.messages;
    seneca.act({ ...extractMessageStats, messages }, res);
  });



}
module.exports = {
  processMessagesPattern,
  processMessagesPlugin
}