namespace CJTour.Application.Interfaces;

public interface IBenchmarkService
{
    void StartTimer(string operationName);
    void StopTimer(string operationName);
    Dictionary<string, TimeSpan> GetResults();
    void ClearResults();
}

public class BenchmarkMetrics
{
    public string OperationName { get; set; } = string.Empty;
    public long TotalCalls { get; set; }
    public long TotalErrors { get; set; }
    public TimeSpan TotalDuration { get; set; }
    public TimeSpan AverageDuration => TotalCalls > 0 ? TimeSpan.FromTicks(TotalDuration.Ticks / TotalCalls) : TimeSpan.Zero;
    public TimeSpan MinDuration { get; set; } = TimeSpan.MaxValue;
    public TimeSpan MaxDuration { get; set; } = TimeSpan.MinValue;
    public double SuccessRate => TotalCalls > 0 ? (double)(TotalCalls - TotalErrors) / TotalCalls * 100 : 0;
    public DateTime LastCall { get; set; }
    public DateTime FirstCall { get; set; }
}

public class BenchmarkReport
{
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public List<BenchmarkMetrics> Metrics { get; set; } = new();
    public TimeSpan TotalDuration => TimeSpan.FromTicks(Metrics.Sum(m => m.TotalDuration.Ticks));
    public long TotalOperations => Metrics.Sum(m => m.TotalCalls);
    public double OverallSuccessRate => TotalOperations > 0 ? (double)(TotalOperations - Metrics.Sum(m => m.TotalErrors)) / TotalOperations * 100 : 0;
}
