using CJTour.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CJTour.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BenchmarkController(IBenchmarkService benchmarkService) : ControllerBase
{   
    [HttpGet("results")]
    public ActionResult GetResults()
    {
        var results = benchmarkService.GetResults();
        return Ok(results);
    }

    [HttpDelete("results")]
    public ActionResult ClearResults()
    {
        benchmarkService.ClearResults();
        return Ok("Results cleared");
    }

    [HttpGet("test")]
    public ActionResult TestBenchmark()
    {
        // Simular una operación lenta
        benchmarkService.StartTimer("TestOperation");
        
        Thread.Sleep(1000); // Esperar 1 segundo
        
        benchmarkService.StopTimer("TestOperation");
        
        return Ok("Test completed");
    }
}