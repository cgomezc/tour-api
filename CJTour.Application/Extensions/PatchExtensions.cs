namespace CJTour.Application.Extensions;

/// <summary>
/// Provides helpers for applying partial updates to entities.
/// </summary>
public static class PatchExtensions
{
    /// <summary>
    /// Copies non-null properties from <paramref name="source"/> to <paramref name="target"/>.
    /// Properties must have matching names and compatible types.
    /// </summary>
   public static void PatchFrom<TTarget, TSource>(this TTarget target, TSource source)
{
    var targetProps = typeof(TTarget).GetProperties()
        .Where(p => p.CanWrite)
        .ToDictionary(p => p.Name);

    foreach (var sourceProp in typeof(TSource).GetProperties())
    {
        var value = sourceProp.GetValue(source);
        if (value == null) continue;

        if (!targetProps.TryGetValue(sourceProp.Name, out var targetProp))
            continue;

        // Caso directo: tipos compatibles
        if (targetProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
        {
            targetProp.SetValue(target, value);
            continue;
        }

        // Caso colección: IEnumerable<T> (array, List<T>, etc.) -> List<T>
        if (value is System.Collections.IEnumerable enumerable and not string
            && targetProp.PropertyType.IsGenericType
            && targetProp.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
        {
            var elementType = targetProp.PropertyType.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(elementType);
            var list = Activator.CreateInstance(listType);
            var addMethod = listType.GetMethod("Add");
            foreach (var item in enumerable)
                addMethod!.Invoke(list, new[] { item });

            targetProp.SetValue(target, list);
        }
    }
}


}
