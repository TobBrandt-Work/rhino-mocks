using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_MarkHildreth
    {
        [Fact]
        public void CanStubMyEntity()
        {
            MyEntity stub = MockRepository.Partial<MyEntity>();
            stub.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            Assert.NotNull(stub);
        }

        public class Entity
        {
            private int id;
            public virtual int ID
            {
                get { return id; }
                // Here's the trick.. don't make this protected and all is well.
                protected set { id = value; }
            }

            public virtual int Id2
            {
                protected get { return id; }
                set { id = value; }
            }

            public virtual int Id3
            {
                get { return id; }
                private set { id = value; }
            }

            public virtual int Id4
            {
                private get { return id; }
                set { id = value; }
            }
        }

        public class MyEntity : Entity
        {
        }
    }
}