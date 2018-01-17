using System;
using Xunit;

namespace Rhino.Mocks.Tests
{

    public class CustomAttributesOnMocks
    {
        [Fact]
        public void Mock_will_have_Protect_attriute_defined_on_them()
        {
            var disposable = MockRepository.Mock<IDisposable>();
            disposable.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            Assert.True(disposable.GetType().IsDefined(typeof(__ProtectAttribute), true));
        }
    }
}