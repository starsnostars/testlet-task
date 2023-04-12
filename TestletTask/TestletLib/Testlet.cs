namespace TestletLib
{
  public class Testlet
  {
    public string TestletId;
    private List<Item> Items;
    
    public Testlet(string testletId, List<Item> items)
    {
      TestletId = testletId;
      Items = items;
    }

    public List<Item> Randomize()
    {
      // Items private collection has 6 Operational and 4 Pretest Items.
      // Randomize the order of these items as per the requirement(with TDD)
      // The assignment will be reviewed on the basis of – Tests written first, Correct
      // logic, Well structured & clean readable cod
      var random = new Random();

      // Shuffle using Fisher-Yates algorithm
      for (int i = Items.Count - 1; i >= 0; i--)
      {
        var idx = random.Next(i + 1);

        var item = Items[idx];
        Items[idx] = Items[i];
        Items[i] = item;
      }

      var head = 0;
      // Move a couple of pretest items to the front
      for (int i = 0; i < Items.Count; i++)
      {
        // First two items are pretest
        if (head == 1)
        {
          break;
        }

        if (Items[i].ItemType == ItemTypeEnum.Pretest)
        {
          var tmp = Items[head];
          Items[head] = Items[i];
          Items[i] = tmp;
          head++;
        }
      }

      return Items;
    }
  }

  public class Item
  {
    public string ItemId;
    public ItemTypeEnum ItemType;
  }

  public enum ItemTypeEnum
  {
    Pretest = 0,
    Operational = 1
  }
}
