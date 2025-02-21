using RangeApp;
using Xunit.Abstractions;
namespace RangeAppTest
{
    public class RoundRepositoryTest
    {
        private readonly ITestOutputHelper output;

        public RoundRepositoryTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GetRoundIDTest()
        {
            var repo = new RangeApp.Models.RoundRepository("test.db3");
            repo.ClearRoundTable();
            var round = new RangeApp.Models.Round();
            round.Name = "One";
            Assert.True(1 == repo.AddNewRound(round));
            var round2 = new RangeApp.Models.Round();
            round2.Name = "Two";
            Assert.True(1 == repo.AddNewRound(round2));

            var roundList = repo.GetRounds();
            Assert.Equal(roundList.Count, 2);

            var id = repo.GetRoundId(roundList[0]);
            output.WriteLine("id {0}, roundId {1}", id, roundList[0].Id);
            Assert.Equal(id, roundList[0].Id);
            Assert.NotEqual(id, roundList[1].Id);

            var id2 = repo.GetRoundId(roundList[1]);
            output.WriteLine("id {0}, roundId {1}", id2, roundList[1].Id);
            Assert.Equal(id2, roundList[1].Id);
            Assert.NotEqual(id2, roundList[0].Id);

            // round that is not in database
            var badRound = new RangeApp.Models.Round();
            Assert.Equal(-1, repo.GetRoundId(badRound));
        }

        [Fact]
        public void Test2()
        {
            var vm = new RangeApp.Models.RoundRepository("test.db3");
            Assert.True(vm.GetBullets().Count == 0);
        }
    }
}
