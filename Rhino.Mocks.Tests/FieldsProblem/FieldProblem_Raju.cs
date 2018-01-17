using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_Raju
    {
        public class A
        {
            private int _a, _b;
            public int a { get { return _a; } set { _a = value; } }
            public int b { get { return _b; } set { _b = value; } }
        }

        public interface MyInterface
        {
            int retValue(A a);
        }

        public class MyClass : MyInterface
        {
            public virtual int retValue(A a)
            {
                int i = 5;
                return i;
            }
        }
        [Fact]
        public void TestMethod1()
        {
            A a = new A();
            a.a = 10;
            a.b = 12;

            MyInterface myInterface = MockRepository.Mock<MyInterface>();
            myInterface.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            myInterface.Expect(x => x.retValue(Arg<A>.Matches(m => m.a == 10 && m.b == 12)))
                .Return(5);

            int ret = myInterface.retValue(a);

            Assert.True(ret == 5);
            myInterface.VerifyExpectations(true);
        }
    }
}