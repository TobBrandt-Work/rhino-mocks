using System;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_Adam
    {
        public interface IFoo
        {
            string Str { get; set; }
            event EventHandler Event;
        }

        [Fact]
        public void ShouldRaiseEventWhenEverPropIsSet()
        {
            var foo = MockRepository.Mock<IFoo>();
            foo.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            foo.Stub(x => x.Str = Arg<string>.Is.Anything)
                .WhenCalled(() => foo.Raise(y => y.Event += null,
                    new object[] { foo, EventArgs.Empty }));

            int calls = 0;
            foo.Event += delegate
            {
                calls += 1;
            };

            foo.Str = "1";
            foo.Str = "2";
            foo.Str = "3";
            foo.Str = "4";

            Assert.Equal(4, calls);
        }
    }
}