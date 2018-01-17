using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_Christian
    {
        [Fact]
        public void PropertiesWillBehaveLikeProperties()
        {
            TestObject testObject = MockRepository.Mock<TestObject>();
            testObject.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            Assert.Equal(0, testObject.IntProperty);
        }
    }

    public class TestObject
    {
        private int _intProperty;

        public virtual int IntProperty
        {
            get { return _intProperty; }
            set { _intProperty = value; }
        }
    }
}