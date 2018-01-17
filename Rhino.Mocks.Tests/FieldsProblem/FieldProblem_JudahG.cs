using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{

    public class FieldProblem_JudahG
    {
        public interface IView
        {
            int? Foo { get; set; }
        }

        [Fact]
        public void IsMatching()
        {
            //Predicate<int> alwaysReturnsTrue = delegate(int input)
            //{
            //    return true;
            //};

            IView view = MockRepository.Mock<IView>();
            view.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            view.Expect(x => x.Foo = Arg<int>.Matches(y => true));
            //.Constraints(Is.Matching(alwaysReturnsTrue));

            view.Foo = 1;
            view.VerifyExpectations(true);
        }
    }
}