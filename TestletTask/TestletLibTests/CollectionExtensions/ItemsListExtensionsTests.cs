using TestletLib.Models;
using TestletLib;
using Xunit;
using TestletLibTests.Utils;

namespace TestletLibTests.CollectionExtensions
{
  public class ItemsListExtensionsTests : IClassFixture<SharedTestItemsFixture>
  {
    private readonly SharedTestItemsFixture itemsProvider;

    public ItemsListExtensionsTests(SharedTestItemsFixture itemsProvider)
    {
      this.itemsProvider = itemsProvider;
    }

    [Fact]
    public void Shuffle_NormalFlow_DoesNotMutateInputData()
    {
      var data = this.itemsProvider.GetTestItems();
      var sut = new Testlet("1", data);

      var res = sut.Randomize();

      var mutatedItems = res.Where(i => !data.Any(d => d.ItemId == i.ItemId));

      Assert.Empty(mutatedItems);
    }

    [Fact]
    public void Shuffle_NormalFlow_DoesNotHaveDuplicates()
    {
      var sut = new Testlet("1", this.itemsProvider.GetTestItems());

      var res = sut.Randomize();

      var duplicates = res.GroupBy(g => g.ItemId)
          .Where(g => g.Count() > 1);

      Assert.Empty(duplicates);
    }

    [Fact]
    public void Shuffle_NormalFlow_FirstTwoItemsAreAlwaysPretest()
    {
      var sut = new Testlet("1", this.itemsProvider.GetTestItems());

      var res = sut.Randomize();

      Assert.Equal(ItemType.Pretest, res[0].ItemType);
      Assert.Equal(ItemType.Pretest, res[1].ItemType);
    }

    [Fact]
    public void Shuffle_NormalFlow_ContainsFourPretestItems()
    {
      var sut = new Testlet("1", this.itemsProvider.GetTestItems());

      var res = sut.Randomize();

      Assert.Equal(4, res.Count(i => i.ItemType == ItemType.Pretest));
    }

    [Fact]
    public void Shuffle_NormalFlow_ItemsAreInRandomOrder()
    {
      var data = this.itemsProvider.GetTestItems();
      var sut = new Testlet("1", data);

      var res = sut.Randomize();

      var sameOrder = AreinSameOrder(data, res);

      Assert.False(sameOrder);
    }

    [Fact]
    public void Shuffle_CalledMultipleTimes_ReturnsItemsInDifferentOrderOnEachCall()
    {
      var sut = new Testlet("1", this.itemsProvider.GetTestItems());

      var a = sut.Randomize();
      var b = sut.Randomize();

      var sameOrder = AreinSameOrder(a, b);

      Assert.False(sameOrder);
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
