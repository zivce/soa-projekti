// @ts-check
"use strict";

const seneca = require("seneca")();

const actOnMathSum = (err, result) => {
  if (err) return console.error(err);
  console.log(result);
};

seneca.add("role:math,cmd:sum", (msg, reply) => {
  reply(null, { answer: msg.left + msg.right });
});

seneca.act({ role: "math", cmd: "sum", left: 1, right: 2 }, actOnMathSum);
