using TestletLib.CollectionExtensions;
using TestletLib.Core;
using TestletLib.Models;

namespace TestletLib
{
  public class Testlet
  {
    private List<Item> _items;

    public string TestletId { get; set; }
    
    public Testlet(string testletId, List<Item> items)
    {
      ValidateItems(items);

      TestletId = testletId;
      _items = items;
    }

    public List<Item> Randomize()
    {
      var warmup = _items
        .Where(i => i.ItemType == ItemType.Pretest)
        .Shuffle()
        .Take(Constants.WarmupItemsCount)
        .ToList();

      var main = _items
        .Where(i => !warmup.Contains(i))
        .Shuffle()
        .Take(Constants.MaxItemsCount - Constants.WarmupItemsCount);

      return warmup.Concat(main).ToList();
    }

    private static void ValidateItems(List<Item> items)
    {
      if (items.Count(i => i.ItemType == ItemType.Pretest) != Constants.PretestItemsCount ||
        items.Count(i => i.ItemType == ItemType.Operational) != Constants.OperationalItemsCount)
      {
        throw new ArgumentException(
          $"{nameof(items)} should contain {Constants.PretestItemsCount} pretest and {Constants.OperationalItemsCount} operational items");
      }
    }
  }
}
