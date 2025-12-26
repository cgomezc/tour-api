using CJTour.Application.Interfaces;
using System.Diagnostics;

namespace CJTour.Application.Services;

public class BenchmarkService : IBenchmarkService
{
    private readonly Dictionary<string, Stopwatch> _timers = [];
    private readonly Dictionary<string, TimeSpan> _results =[];

    public void StartTimer(string operationName)
    {
        var stopwatch = Stopwatch.StartNew();
        _timers[operationName] = stopwatch;
    }

    public void StopTimer(string operationName)
    {
        if (_timers.TryGetValue(operationName, out var stopwatch))
        {
            stopwatch.Stop();
            _results[operationName] = stopwatch.Elapsed;
            _timers.Remove(operationName);
        }
    }

    public Dictionary<string, TimeSpan> GetResults()
    {
        return new Dictionary<string, TimeSpan>(_results);
    }

    public void ClearResults()
    {
        _results.Clear();
        _timers.Clear();
    }
}