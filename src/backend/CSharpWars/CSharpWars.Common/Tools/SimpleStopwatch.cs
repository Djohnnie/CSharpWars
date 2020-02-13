using System;
using System.Diagnostics;

namespace CSharpWars.Common.Tools
{
    public class SimpleStopwatch : IDisposable
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;

        public SimpleStopwatch()
        {
            _stopwatch.Start();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
        }
    }
}