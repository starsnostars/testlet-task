using TestletLib.Core;
using TestletLib.Models;

namespace TestletLibTests.Utils
{
  public class SharedTestItemsFixture
  {
    public List<Item> GetTestItems(
      int operational = Constants.OperationalItemsCount,
      int pretest = Constants.PretestItemsCount)
    {
      return Enumerable
        .Range(0, operational)
        .Select(i => new Item
        {
          ItemId = i.ToString(),
          ItemType = ItemType.Operational
        })
        .Concat(
          Enumerable
            .Range(0, pretest)
            .Select(i => new Item
            {
              ItemId = (i + operational).ToString(),
              ItemType = ItemType.Pretest
            })
        )
        .ToList();
    }
  }
}
