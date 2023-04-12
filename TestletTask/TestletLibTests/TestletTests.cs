using TestletLib;
using Xunit;

namespace TestletLibTests
{
  public class TestletTests
  {
    private readonly Random random = new Random();

    [Fact]
    public void Randomize_NormalFlow_DoesNotHaveDuplicates()
    {
      var sut = new Testlet("1", GetTestItems());

      var res = sut.Randomize();

      var duplicates = res.GroupBy(g => g.ItemId)
          .Where(g => g.Count() > 1);

      Assert.Empty(duplicates);
    }

    [Fact]
    public void Randomize_NormalFlow_FirstTwoItemsAreAlwaysPretest()
    {
      var sut = new Testlet("1", GetTestItems());

      var res = sut.Randomize();

      Assert.Equal(ItemTypeEnum.Pretest, res[0].ItemType);
      Assert.Equal(ItemTypeEnum.Pretest, res[1].ItemType);
    }

    [Fact]
    public void Randomize_NormalFlow_ContainsFourPretestItems()
    {
      var sut = new Testlet("1", GetTestItems());

      var res = sut.Randomize();

      Assert.Equal(4, res.Count(i => i.ItemType == ItemTypeEnum.Pretest));
    }

    [Fact]
    public void Randomize_NormalFlow_ItemsAreInRandomOrder()
    {
      var data = GetTestItems();
      var sut = new Testlet("1", GetTestItems());

      var res = sut.Randomize();

      var sameOrder = AreinSameOrder(data, res);

      Assert.False(sameOrder);
    }

    private List<Item> GetTestItems(int count = 10)
    {
      return Enumerable
        .Range(0, 6)
        .Select(i => new Item 
          { 
            ItemId = i.ToString(),
            ItemType = ItemTypeEnum.Operational
          })
        .Concat(
          Enumerable
            .Range(0, 4)
            .Select(i => new Item
              {
                ItemId = (i + 6).ToString(),
                ItemType = ItemTypeEnum.Pretest
              })
        )
        .ToList();
    }

    private bool AreinSameOrder(IEnumerable<Item> a, IEnumerable<Item> b)
    {
      var length = Math.Min(a.Count(), b.Count());

      for (int i = 0; i < length; i++)
      {
        if (a.ElementAt(i).ItemId != b.ElementAt(i).ItemId)
        {
          return false;
        }
      }

      return true;
    }
  }
}
