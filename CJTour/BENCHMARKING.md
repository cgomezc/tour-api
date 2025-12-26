# CJTour Benchmarking System - Básico

## ¿Qué es?

Un sistema simple para medir el tiempo que tardan las operaciones en el TourController.

## ¿Cómo funciona?

### 1. Interface Simple
```csharp
public interface IBenchmarkService
{
    void StartTimer(string operationName);     // Inicia el cronómetro
    void StopTimer(string operationName);      // Para el cronómetro
    Dictionary<string, TimeSpan> GetResults(); // Obtiene los resultados
    void ClearResults();                       // Limpia los resultados
}
```

### 2. Implementación
```csharp
public class BenchmarkService : IBenchmarkService
{
    private readonly Dictionary<string, Stopwatch> _timers = new();
    private readonly Dictionary<string, TimeSpan> _results = new();

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
}
```

### 3. Uso en el Controller
```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<TourDto>>> GetTours()
{
    _benchmarkService.StartTimer("GetTours");  // Inicia cronómetro
    
    try
    {
        var tours = await _tourService.GetAllToursAsync();
        return Ok(tours);
    }
    finally
    {
        _benchmarkService.StopTimer("GetTours"); // Para cronómetro
    }
}
```

## Endpoints Disponibles

### Benchmark Controller
- `GET /api/benchmark/results` - Ver todos los tiempos medidos
- `DELETE /api/benchmark/results` - Limpiar todos los resultados
- `GET /api/benchmark/test` - Probar el benchmark (espera 1 segundo)

### TourController (con benchmark integrado)
- `GET /api/tours` - Mide tiempo de obtener tours
- `GET /api/tours/featured` - Mide tiempo de obtener tours destacados
- `GET /api/tours/{id}` - Mide tiempo de obtener tour por ID
- etc.

## Ejemplo de Uso

1. **Hacer algunas peticiones:**
   ```
   GET /api/tours
   GET /api/tours/featured
   GET /api/tours/1
   ```

2. **Ver los resultados:**
   ```
   GET /api/benchmark/results
   ```

3. **Respuesta:**
   ```json
   {
     "GetTours": "00:00:00.1500000",
     "GetFeaturedTours": "00:00:00.0800000", 
     "GetTourById_1": "00:00:00.0500000"
   }
   ```

## ¿Por qué es útil?

- **Identificar operaciones lentas** - Ver qué endpoints tardan más
- **Optimización** - Saber dónde enfocar esfuerzos de mejora
- **Monitoreo** - Ver si el rendimiento empeora con el tiempo
- **Debugging** - Entender por qué algunas operaciones son lentas

## Próximos Pasos (para crecer)

1. **Contador de llamadas** - Cuántas veces se llamó cada operación
2. **Promedios** - Tiempo promedio por operación
3. **Mínimo/Máximo** - Tiempos más rápidos y más lentos
4. **Persistencia** - Guardar resultados en base de datos
5. **Alertas** - Notificar cuando una operación es muy lenta

Este sistema básico te permite entender cómo funciona el benchmarking y luego puedes expandirlo según tus necesidades. 