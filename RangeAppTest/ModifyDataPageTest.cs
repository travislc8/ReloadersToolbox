using RangeApp;

namespace RangeAppTest
{
    public class ModifyDataPageTest
    {
        [Fact]
        public void Test1()
        {
            var vm = new RangeApp.Models.RoundRepository("test.db3");
            Assert.True(vm.GetBullets().Count == 0);
            var bullet = new RangeApp.Models.Bullet();
            int result = vm.AddNewBullet(bullet);
            Assert.True(result == 1);
            result = vm.DeleteBullet(bullet);
            Assert.True(result == 1);
        }

        [Fact]
        public void Test2()
        {
            var vm = new RangeApp.Models.RoundRepository("test.db3");
            Assert.True(vm.GetBullets().Count == 0);
        }
    }
}
