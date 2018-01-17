using System;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public interface IMyService
    {
        void Func1();
        void Func2();
        void Func3();
    }

    public class FieldProblem_oblomov : IDisposable
    {
        IMyService service;

        public FieldProblem_oblomov()
        {
            service = MockRepository.Mock<IMyService>();
            service.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
        }

        public void Dispose()
        {
            service.VerifyExpectations(true);
        }

        [Fact]
        public void TestWorks()
        {
            service.Expect(x => x.Func1());
            service.Expect(x => x.Func2());
            service.Expect(x => x.Func3());

            service.Func2();
            service.Func1();
            service.Func3();
        }

        [Fact]
        public void TestDoesnotWork()
        {
            service.Expect(x => x.Func3());

            service.Func3();
        }

        [Fact]
        public void TestDoesnotWork2()
        {
            service.Expect(x => x.Func3());

            service.Func3();
        }
    }
}
