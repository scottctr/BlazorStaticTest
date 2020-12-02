using System;

namespace SwitchBoard
{
        public class LogEntry
        {
            private static int _id;

            public LogEntry(string message, DateTime? timestamp = null)
            {
                Id = ++_id;
                Entry = message;
                Timestamp = timestamp ?? DateTime.Now;
            }

            public int Id { get; }
            public DateTime Timestamp { get; }
            public string Entry { get; }
        }
}