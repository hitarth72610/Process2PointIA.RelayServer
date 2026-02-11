using Process2PointIA.RelayServer.Models;
using System.Collections.Concurrent;

namespace Process2PointIA.RelayServer.Storage
{
    public static class RelayStore
    {
        private static readonly ConcurrentDictionary<string, BotState> _states = new();
        private static readonly ConcurrentDictionary<string, BotCommand?> _commands = new();

        // ---- STATUS ----
        public static void UpdateState(string botId, BotState state)
        {
            state.LastUpdated = DateTime.UtcNow;
            _states[botId] = state;
        }

        public static BotState? GetState(string botId)
        {
            _states.TryGetValue(botId, out var state);
            return state;
        }

        // ---- COMMAND ----
        public static void SetCommand(string botId, string command)
        {
            _commands[botId] = new BotCommand { Command = command };
        }

        public static BotCommand? ConsumeCommand(string botId)
        {
            _commands.TryRemove(botId, out var cmd);
            return cmd;
        }
    }
}