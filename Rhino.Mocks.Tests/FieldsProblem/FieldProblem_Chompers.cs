using System.IO;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_Chompers
    {
        [Fact]
        public void MockingPropertyThatReturnsStream()
        {
            IBaseMessagePart messagePart = MockRepository.Mock<IBaseMessagePart>();
            messagePart.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            MemoryStream stream = new MemoryStream();
            messagePart.Expect(x => x.Data)
                .Return(stream)
                .Repeat.Any();

            messagePart.Data.WriteByte(127);
            stream.Seek(0, SeekOrigin.Begin);
            Assert.Equal(127, stream.ReadByte());
        }
    }

    public interface IBaseMessagePart
    {
        Stream Data { get; set; }
    }
}
