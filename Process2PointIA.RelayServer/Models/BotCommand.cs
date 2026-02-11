namespace Process2PointIA.RelayServer.Models
{
    public class BotCommand
    {
        public string Command { get; set; } = "";
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    }
}