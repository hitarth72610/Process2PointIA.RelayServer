namespace Process2PointIA.RelayServer.Models
{
    public class BotState
    {
        public string BotId { get; set; } = "";
        public string Status { get; set; } = "Idle";

        public int TotalClaims { get; set; }
        public int CompletedClaims { get; set; }

        public int CurrentBatch { get; set; }
        public string CurrentTask { get; set; } = "";

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}