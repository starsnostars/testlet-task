using TestletLib;
using TestletLib.Models;
using TestletLibTests.Utils;
using Xunit;

namespace TestletLibTests
{
  public class TestletTests : IClassFixture<SharedTestItemsFixture>
  {
    private readonly SharedTestItemsFixture _itemsProvider;

    public TestletTests(SharedTestItemsFixture itemsProvider)
    {
      _itemsProvider = itemsProvider;
    }

    [Fact]
    public void Ctor_PassedValidArguments_DoesNotThrow()
    {
      var data = _itemsProvider.GetTestItems();

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
      var data = _itemsProvider.GetTestItems(operational, pretest);

      var notEnoughItems = Record.Exception(() => new Testlet("1", data));

      Assert.NotNull(notEnoughItems);
      Assert.IsType<ArgumentException>(notEnoughItems);
    }

    [Fact]
    public void Randomize_NormalFlow_ItemsAreInRandomOrder()
    {
      var data = _itemsProvider.GetTestItems();
      var sut = new Testlet("1", data);

      var res = sut.Randomize();

      var sameOrder = AreInTheSameOrder(data, res);

      Assert.False(sameOrder);
    }

    [Fact]
    public void Randomize_CalledMultipleTimes_ReturnsItemsInDifferentOrderOnEachCall()
    {
      var sut = new Testlet("1", _itemsProvider.GetTestItems());

      var a = sut.Randomize();
      var b = sut.Randomize();

      var sameOrder = AreInTheSameOrder(a, b);

      Assert.False(sameOrder);
    }

    [Fact]
    public void Randomize_NormalFlow_DoesNotMutateInputData()
    {
      var data = _itemsProvider.GetTestItems();
      var sut = new Testlet("1", data);

      var res = sut.Randomize();

      var mutatedItems = res.Where(i => !data.Any(d => d.ItemId == i.ItemId));

      Assert.Empty(mutatedItems);
    }

    [Fact]
    public void Randomize_NormalFlow_DoesNotHaveDuplicates()
    {
      var sut = new Testlet("1", _itemsProvider.GetTestItems());

      var res = sut.Randomize();

      var duplicates = res
        .GroupBy(g => g.ItemId)
        .Where(g => g.Count() > 1);

      Assert.Empty(duplicates);
    }

    [Fact]
    public void Randomize_NormalFlow_FirstTwoItemsAreAlwaysPretest()
    {
      var sut = new Testlet("1", _itemsProvider.GetTestItems());

      var res = sut.Randomize();

      Assert.Equal(ItemType.Pretest, res[0].ItemType);
      Assert.Equal(ItemType.Pretest, res[1].ItemType);
    }

    [Fact]
    public void Randomize_NormalFlow_ContainsFourPretestItems()
    {
      var sut = new Testlet("1", _itemsProvider.GetTestItems());

      var res = sut.Randomize();

      Assert.Equal(4, res.Count(i => i.ItemType == ItemType.Pretest));
    }

    private bool AreInTheSameOrder(IEnumerable<Item> a, IEnumerable<Item> b)
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
