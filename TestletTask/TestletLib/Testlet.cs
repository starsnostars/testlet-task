using TestletLib.Models;

namespace TestletLib
{
  public class Testlet
  {
    public string TestletId;
    private List<Item> Items;
    
    private readonly Random random = new();

    public Testlet(string testletId, List<Item> items)
    {
      ValidateItems(items);

      TestletId = testletId;
      Items = items;
    }

    // For a production environment,
    // I'd suggest creating a separate shuffle
    // service to shuffle collections. 
    // The output of that service would be passed
    // to this class via constructor injection
    // and then the Randomize method could be removed
    // from here. 
    public List<Item> Randomize()
    {
      var res = new List<Item>(Items);

      // Shuffle using Fisher-Yates algorithm
      for (int i = res.Count - 1; i >= 0; i--)
      {
        var idx = this.random.Next(i + 1);

        var item = res[idx];
        res[idx] = res[i];
        res[i] = item;
      }

      var pretestCount = 0;
      // Move a couple of pretest items to the front
      for (int i = 0; i < res.Count; i++)
      {
        // First two items are pretest
        if (pretestCount == 2)
        {
          break;
        }

        if (res[i].ItemType == ItemTypeEnum.Pretest)
        {
          var tmp = res[pretestCount];
          res[pretestCount] = res[i];
          res[i] = tmp;
          pretestCount++;
        }
      }

      return res;
    }

    private static void ValidateItems(IEnumerable<Item> items)
    {
      var pretest = items.Count(i => i.ItemType == ItemTypeEnum.Pretest);
      var operational = items.Count(i => i.ItemType == ItemTypeEnum.Operational);

      if (pretest + operational != 10)
      {
        throw new ArgumentException($"{nameof(items)} should contain ten elements");
      }

      if (pretest != 4)
      {
        throw new ArgumentException($"{nameof(items)} should contain four pretest items");
      }

      if (operational != 6)
      {
        throw new ArgumentException($"{nameof(items)} should contain six operational");
      }
    }
  }
}
