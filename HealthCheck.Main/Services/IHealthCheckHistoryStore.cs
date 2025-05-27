using System;
using System.Collections.Generic;
using HealthCheck.Main.Models;

namespace HealthCheck.Main.Services
{
    public interface IHealthCheckHistoryStore
    {
        void AddPing(HealthCheckPingResult result);
        IReadOnlyList<HealthCheckPingResult> GetHistory(string serviceKey, DateTime since);
        IReadOnlyList<HealthCheckPingResult> GetAllHistory(DateTime since);
    }
}