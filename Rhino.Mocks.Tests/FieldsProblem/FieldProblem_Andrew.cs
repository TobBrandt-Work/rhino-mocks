namespace Rhino.Mocks.Tests.FieldsProblem
{
    using System;
    using System.Data;
    using Exceptions;
    using Rhino.Mocks;
    using Xunit;

    public class FieldProblem_Andrew
    {
        [Fact]
        public void Will_get_unexpect_error()
        {
            var stubConnection = MockRepository.Mock<IDbConnection>();
            stubConnection.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            var mockCommand = MockRepository.Mock<IDbCommand>();
            mockCommand.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            mockCommand.Expect(c => c.Connection = stubConnection);
            mockCommand.Expect(c => c.Connection = null);
            mockCommand.Stub(c => c.ExecuteNonQuery())
                .Throws<TestException>();

            var executor = new Executor(stubConnection);
            try
            {
                executor.ExecuteNonQuery(mockCommand);
                Assert.False(true, "exception was expected");
            }
            catch (TestException)
            {
            }

            Assert.Throws<ExpectationViolationException>(
                () => mockCommand.VerifyAllExpectations());
        }
    }

    public class TestException : Exception
    {
    }


    public class Executor
    {
        private IDbConnection _connection;

        public Executor(IDbConnection connection)
        {
            this._connection = connection;
        }

        public int ExecuteNonQuery(IDbCommand command)
        {
            try
            {
                command.Connection = this._connection;
                return command.ExecuteNonQuery();
            }
            finally
            {
                //command.Connection = null;
                if (this._connection.State != ConnectionState.Closed) this._connection.Close();
            }
        }
    }
}