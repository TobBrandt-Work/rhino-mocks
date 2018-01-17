
using System;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_Libardo
    {
        [Fact]
        public void Can_mix_assert_was_call_with_verify_all()
        {
            var errorHandler = MockRepository.Mock<IErrorHandler>();
            errorHandler.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            var ex = new Exception("Take this");
            errorHandler.HandleError(ex);

            errorHandler.AssertWasCalled(eh => eh.HandleError(ex));

            errorHandler.VerifyExpectations();
        }
    }

    public interface IErrorHandler
    {
        void HandleError(Exception e);
    }
}