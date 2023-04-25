namespace TestletLib.CollectionExtensions
{
  public static class ItemsListExtensions
  {
    private static Random _random = new();

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items)
    {
      return items.OrderBy(_ => _random.Next(items.Count()));
    }
  }
}
