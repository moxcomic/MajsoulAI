const readline = require('readline');
const protobuf = require("protobufjs");

const Type = {
  NOTIFICATION: 1,
  REQUEST: 2,
  RESPONSE: 3,
};

function lookupMethod(root, e) {
  if ("string" == typeof e && (e = e.split(".")),
  0 === e.length)
      return null;
  var t = root.lookupService(e.slice(0, -1));
  if (!t)
      return null;
  var i = e[e.length - 1];
  return t.methods[i].resolve();
}

function getRequestIndex(buffer) {
  return (buffer[2] << 8) + buffer[1];
}

async function main() {
  const root = await protobuf.load("liqi.json");
  const Wrapper = root.lookupType('lq.Wrapper');
  const callMap = new Map();
  let reqCount = 0;
    
  const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout,
    terminal: false
  });

  rl.on('line', (line) => {
    if (!line) return;

    const input = JSON.parse(line);
    
    if (input.action === 'encode') {
      const method = lookupMethod(root, input.methodName);
      callMap.set(reqCount, method);

      const RequestType = method.resolvedRequestType;
      const innerBuffer = RequestType.encode(input.data).finish();
      const buffer = Wrapper.encode({name: input.methodName, data: innerBuffer}).finish();
      const bufferWithHeader = Buffer.concat([new Uint8Array([Type.REQUEST, reqCount & 0xff, reqCount >> 8]), buffer]);

      console.log(bufferWithHeader.toString('hex'));
      reqCount++;
    } else if (input.action === 'decode') {
      try {
        const buffer = Buffer.from(input.data, "hex");
        switch (buffer[0]) {
          case Type.NOTIFICATION:
              const wrappedNotification = Wrapper.decode(buffer.slice(1));
  
              const messageType = root.lookupType(wrappedNotification.name);
              const notification = messageType.decode(wrappedNotification.data);
              
              if (wrappedNotification.name === '.lq.ActionPrototype') {
                decodeActionPrototype(notification.name, notification.data);
              } else {
                console.log(JSON.stringify({
                  success: true,
                  type: "notification",
                  methodName: wrappedNotification.name,
                  data: notification,
                }));
              }
              break;
          case Type.RESPONSE:
              const method = callMap.get(getRequestIndex(buffer));
              const ResponseType = method.resolvedResponseType;
              try {
                const response = ResponseType.decode(buffer.slice(3));
                console.log(JSON.stringify({
                  success: true,
                  type: "response",
                  methodName: method.fullName,
                  data: response,
                }));
              } catch (e) {
                console.log(JSON.stringify({
                  success: false,
                  type: "response",
                  methodName: method.fullName,
                }));
              }

              break;
        }
      } catch (e) {
        console.log(JSON.stringify({
          success: false,
          error: e.message,
        }));
      }
    } else if (input.action === "decodeActionPrototype") {
      decodeActionPrototype(input.actionName, Buffer.from(input.data, "hex"));
    }
  });

  function decodeActionPrototype(name, data) {
    const ActionType = root.lookupType(name);
    const actionData = ActionType.decode(data);
    console.log(JSON.stringify({
      success: true,
      type: "notification",
      methodName: name,
      data: actionData,
    }));
  }
}

main();