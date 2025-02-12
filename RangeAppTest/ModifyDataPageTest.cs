using RangeApp;

namespace RangeAppTest
{
    public class ModifyDataPageTest
    {
        [Fact]
        public void Test1()
        {
            var vm = new RangeApp.Models.RoundRepository("test");
            var bullet = new RangeApp.Models.Bullet();
            int result = vm.AddNewBullet(bullet);
            Assert.True(result == 1);
            result = vm.DeleteBullet(bullet);
            Assert.True(result == 1);
        }
    }
}
