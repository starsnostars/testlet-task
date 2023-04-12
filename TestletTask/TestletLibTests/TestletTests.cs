using TestletLib;
using TestletLib.Models;
using Xunit;

namespace TestletLibTests
{
  public class TestletTests
  {
    [Fact]
    public void Ctor_PassedValidArguments_DoesNotThrow()
    {
      var data = GetTestItems();

      var exception = Record.Exception(() => new Testlet("1", data));

      Assert.Null(exception);
    }

    [Theory]
    [InlineData(3, 0)]
    [InlineData(1, 9)]
    [InlineData(6, 10)]
    [InlineData(0, 0)]
    public void Ctor_PassedInvalidArguments_ThrowsArgumentException(int operational, int pretest)
    {
      var data = GetTestItems(operational, pretest);

      var notEnoughItems = Record.Exception(() => new Testlet("1", data));

      Assert.NotNull(notEnoughItems);
      Assert.IsType<ArgumentException>(notEnoughItems);
    }

    [Fact]
    public void Randomize_NormalFlow_DoesNotMutateInputData()
    {
      var data = GetTestItems();
      var sut = new Testlet("1", GetTestItems());
      
      var res = sut.Randomize();

      var mutatedItems = res.Where(i => !data.Any(d => d.ItemId == i.ItemId));

      Assert.Empty(mutatedItems);
    }

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
      var sut = new Testlet("1", data);

      var res = sut.Randomize();

      var sameOrder = AreinSameOrder(data, res);

      Assert.False(sameOrder);
    }

    [Fact]
    public void Randomize_CalledMultipleTimes_ReturnsItemsInDifferentOrderOnEachCall()
    {
      var sut = new Testlet("1", GetTestItems());

      var a = sut.Randomize();
      var b = sut.Randomize();

      var sameOrder = AreinSameOrder(a, b);

      Assert.False(sameOrder);
    }

    private List<Item> GetTestItems(int operational = 6, int pretest = 4)
    {
      return Enumerable
        .Range(0, operational)
        .Select(i => new Item 
          { 
            ItemId = i.ToString(),
            ItemType = ItemTypeEnum.Operational
          })
        .Concat(
          Enumerable
            .Range(0, pretest)
            .Select(i => new Item
              {
                ItemId = (i + operational).ToString(),
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
