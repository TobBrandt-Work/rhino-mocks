
using System;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{

    public class FieldProblem_TrueWill
    {
        [Fact]
        public void ReadWritePropertyBug1()
        {
            ISomeThing thing = MockRepository.Mock<ISomeThing>();
            thing.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            thing.Number = 21;

            thing.Stub(x => x.Name)
                .Return("Bob");

            Assert.Equal(thing.Number, 21);
            // Fails - calling Stub on anything after
            // setting property resets property to default.
        }

        [Fact(Skip = "Test No Longer Valid")]
        public void ReadWritePropertyBug2()
        {
            ISomeThing thing = MockRepository.Mock<ISomeThing>();
            thing.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            Assert.Throws<InvalidOperationException>(
                () => thing.Stub(x => x.Number).Return(21));
            // InvalidOperationException :
            // Invalid call, the last call has been used...
            // This broke a test on a real project when a
            // { get; } property was changed to { get; set; }.
        }
    }

    public interface ISomeThing
    {
        string Name { get; }

        int Number { get; set; }
    }

}