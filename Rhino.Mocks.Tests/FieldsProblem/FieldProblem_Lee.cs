
using System;
using System.Collections.Generic;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_Lee
    {
        [Fact]
        public void IgnoringArgumentsOnGenericMethod()
        {
            IHaveGenericMethod mock = MockRepository.Mock<IHaveGenericMethod>();
            mock.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            mock.Expect(x => x.Foo(15))
                .IgnoreArguments()
                .Return(true);

            bool result = mock.Foo(16);
            Assert.True(result);
            mock.VerifyAllExpectations();
        }

        [Fact]
        public void WithGenericMethods()
        {
            List<Guid> results = new List<Guid>();

            IFunkyList<int> list = MockRepository.Mock<IFunkyList<int>>();
            list.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            Assert.NotNull(list);

            list.Expect(x => x.FunkItUp<Guid>(Arg.Is("1"), Arg.Is(2)))
                .Return(results);

            Assert.Same(results, list.FunkItUp<Guid>("1", 2));
        }
    }

    public interface IFunkyList<T> : IList<T>
    {
        ICollection<T2> FunkItUp<T2>(object arg1, object arg2);
    }

    public interface IHaveGenericMethod
    {
        bool Foo<T>(T obj);
    }
}