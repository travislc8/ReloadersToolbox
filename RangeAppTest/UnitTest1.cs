using RangeApp;

namespace RangeAppTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.True(Model.Utils.Validate.IntFromString("10"));
        }
    }
}