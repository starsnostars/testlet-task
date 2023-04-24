using TestletLib.Core;
using TestletLib.Models;

namespace TestletLib.CollectionExtensions
{
  public static class ItemsListExtensions
  {
    private static Random random = new();

    public static List<Item> Shuffle(this List<Item> items)
    {
      var warmup = items
        .Where(i => i.ItemType == ItemType.Pretest)
        .Take(Constants.WarmupItemsCount)
        .OrderBy(_ => random.Next(items.Count))
        .ToList();

      var main = items
        .Where(i => !warmup.Contains(i))
        .Take(Constants.MaxItemsCount - Constants.WarmupItemsCount)
        .OrderBy(_ => random.Next(items.Count));

      return warmup.Concat(main).ToList();
    }
  }
}
