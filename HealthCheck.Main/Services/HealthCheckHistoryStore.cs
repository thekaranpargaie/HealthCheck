using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using HealthCheck.Main.Models;

namespace HealthCheck.Main.Services
{
    public class HealthCheckHistoryStore : IHealthCheckHistoryStore
    {
        private readonly ConcurrentQueue<HealthCheckPingResult> _history = new();

        public void AddPing(HealthCheckPingResult result)
        {
            _history.Enqueue(result);
            // Remove entries older than 1 day
            var cutoff = DateTime.UtcNow.AddDays(-1);
            while (_history.TryPeek(out var oldest) && oldest.Timestamp < cutoff)
            {
                _history.TryDequeue(out _);
            }
        }

        public IReadOnlyList<HealthCheckPingResult> GetHistory(string serviceKey, DateTime since)
        {
            return _history.Where(x => x.ServiceKey == serviceKey && x.Timestamp >= since).ToList();
        }

        public IReadOnlyList<HealthCheckPingResult> GetAllHistory(DateTime since)
        {
            return _history.Where(x => x.Timestamp >= since).ToList();
        }
    }
}