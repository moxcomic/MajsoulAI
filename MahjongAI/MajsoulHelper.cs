using Google.Protobuf;

namespace MahjongAI
{
    class MajsoulMessage
    {
        public bool Success { get; set; } = true;
        public MajsoulMessageType Type { get; set; }
        public string MethodName { get; set; }
        //public JToken Json { get; set; }
        public dynamic Data { get; set; }

        public IMessage Message { get; set; }
    }

    enum MajsoulMessageType
    {
        REQUEST,
        RESPONSE,
        NOTIFICATION,
    }
}
