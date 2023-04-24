using TestletLib.CollectionExtensions;
using TestletLib.Core;
using TestletLib.Models;

namespace TestletLib
{
  public class Testlet
  {
    private List<Item> items;

    public string TestletId { get; set; }
    
    public Testlet(string testletId, List<Item> items)
    {
      ValidateItems(items);

      TestletId = testletId;
      this.items = items;
    }

    public List<Item> Randomize()
    {
      return this.items.Shuffle();
    }

    private static void ValidateItems(List<Item> items)
    {
      if (items.Count != Constants.MaxItemsCount)
      {
        throw new ArgumentException($"{nameof(items)} should contain {Constants.MaxItemsCount} elements");
      }

      if (items.Count(i => i.ItemType == ItemType.Pretest) != Constants.PretestItemsCount)
      {
        throw new ArgumentException($"{nameof(items)} should contain {Constants.PretestItemsCount} pretest items");
      }
    }
  }
}
