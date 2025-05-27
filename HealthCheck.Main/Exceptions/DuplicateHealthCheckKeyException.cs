using System;

namespace HealthCheck.Main.Exceptions
{
    public class DuplicateHealthCheckKeyException : Exception
    {
        public DuplicateHealthCheckKeyException(string message) : base(message) { }
    }
}