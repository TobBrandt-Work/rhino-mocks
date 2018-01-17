using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_SamW
    {
        [Fact]
        public void UsingArrayAndOutParam()
        {
            string b;

            ITest test = MockRepository.Mock<ITest>();
            test.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            test.Expect(x => x.ArrayWithOut(Arg<string[]>.List.IsIn("data"), out Arg<string>.Out("SuccessWithOut2").Dummy))
                .Return("SuccessWithOut1");

            string result = test.ArrayWithOut(new string[] { "data" }, out b);

            Assert.Equal("SuccessWithOut2", b);
            Assert.Equal("SuccessWithOut1", result);
        }


        public interface ITest
        {
            string ArrayWithOut(string[] a, out string b);
        }
    }
}
