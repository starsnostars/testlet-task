using TestletLib;
using TestletLibTests.Utils;
using Xunit;

namespace TestletLibTests
{
  public class TestletTests : IClassFixture<SharedTestItemsFixture>
  {
    private readonly SharedTestItemsFixture itemsProvider;

    public TestletTests(SharedTestItemsFixture itemsProvider)
    {
      this.itemsProvider = itemsProvider;
    }

    [Fact]
    public void Ctor_PassedValidArguments_DoesNotThrow()
    {
      var data = this.itemsProvider.GetTestItems();

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
      var data = this.itemsProvider.GetTestItems(operational, pretest);

      var notEnoughItems = Record.Exception(() => new Testlet("1", data));

      Assert.NotNull(notEnoughItems);
      Assert.IsType<ArgumentException>(notEnoughItems);
    }
  }
}
