#region license

// Copyright (c) 2007 Ivan Krivyakov (ivan@ikriv.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using Rhino.Mocks.Exceptions;
using Xunit;

namespace Rhino.Mocks.Tests
{

    public class StrictMockTests
    {
        public class TestClass : MarshalByRefObject
        {
            public TestClass(string unused)
            {
                throw new InvalidCastException("Real method should never be called");
            }

            public void Method()
            {
                throw new InvalidCastException("Real method should never be called");
            }

            public int MethodReturningInt()
            {
                throw new InvalidCastException("Real method should never be called");
            }

            public string MethodReturningString()
            {
                throw new InvalidCastException("Real method should never be called");
            }

            public string MethodGettingParameters(int intParam, string stringParam)
            {
                throw new InvalidCastException("Real method should never be called");
            }

            public void MethodAcceptingTestClass(TestClass other)
            {
                throw new InvalidCastException("Real method should never be called");
            }

            public int GenericMethod<T>(string parameter)
            {
                throw new InvalidCastException("Real method should never be called");
            }

            public T GenericMethodReturningGenericType<T>(string parameter)
            {
                throw new InvalidCastException("Real method should never be called");
            }

            public T GenericMethodWithGenericParam<T>(T parameter)
            {
                throw new InvalidCastException("Real method should never be called");
            }

            public string StringProperty
            {
                get
                {
                    throw new InvalidCastException("Real method should never be called");
                }
                set
                {
                    throw new InvalidCastException("Real method should never be called");
                }
            }
        }

        public class GenericTestClass<T> : MarshalByRefObject
        {
            public int Method(T parameter)
            {
                throw new InvalidCastException("Real method should never be called");
            }

            public U GenericMethod<U>(T parameter)
            {
                throw new InvalidCastException("Real method should never be called");
            }
        }

        [Fact]
        public void CanMockVoidMethod()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.Method());

            t.Method();
            t.VerifyAllExpectations();
        }

        [Fact]
        public void ThrowOnUnexpectedVoidMethod()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Method();

            Assert.Throws<ExpectationViolationException>(
                () => t.VerifyExpectations(true));
        }

        [Fact]
        public void CanMockMethodReturningInt()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.MethodReturningInt())
                .Return(42);

            Assert.Equal(42, t.MethodReturningInt());
            t.VerifyAllExpectations();
        }

        [Fact]
        public void CanMockMethodReturningString()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.MethodReturningString())
                .Return("foo");

            Assert.Equal("foo", t.MethodReturningString());
            t.VerifyAllExpectations();
        }

        [Fact]
        public void CanMockMethodGettingParameters()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.MethodGettingParameters(42, "foo"))
                .Return("bar");

            Assert.Equal("bar", t.MethodGettingParameters(42, "foo"));
            t.VerifyAllExpectations();
        }

        [Fact]
        public void CanRejectIncorrectParameters()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.MethodGettingParameters(42, "foo"))
                .Return("bar");

            t.MethodGettingParameters(19, "foo");

            Assert.Throws<ExpectationViolationException>(
                () => t.VerifyExpectations(true));
        }

        [Fact]
        public void CanMockPropertyGet()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.StringProperty)
                .Return("foo");

            Assert.Equal("foo", t.StringProperty);
            t.VerifyAllExpectations();
        }

        [Fact]
        public void CanMockPropertySet()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.StringProperty = "foo");

            t.StringProperty = "foo";
            t.VerifyAllExpectations();
        }

        [Fact]
        public void CanRejectIncorrectPropertySet()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.StringProperty = "foo");

            t.StringProperty = "bar";

            Assert.Throws<ExpectationViolationException>(
                () => t.VerifyExpectations(true));
        }

        [Fact]
        public void CanMockGenericClass()
        {
            GenericTestClass<string> t = MockRepository.Mock<GenericTestClass<string>>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.Method("foo"))
                .Return(42);

            Assert.Equal(42, t.Method("foo"));
            t.VerifyAllExpectations();
        }

        [Fact]
        public void CanMockGenericMethod()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.GenericMethod<string>("foo"))
                .Return(42);

            Assert.Equal(42, t.GenericMethod<string>("foo"));
            t.VerifyAllExpectations();
        }

        [Fact]
        public void CanMockGenericMethod_WillErrorOnWrongType()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.GenericMethod<string>("foo"))
                .Return(42);

            //Assert.Equal(42, t.GenericMethod<int>("foo"));
            t.GenericMethod<int>("foo");

            Assert.Throws<ExpectationViolationException>(
                () => t.VerifyExpectations(true));
        }

        [Fact]
        public void CanMockGenericMethodReturningGenericType()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.GenericMethodReturningGenericType<string>("foo"))
                .Return("bar");

            Assert.Equal("bar", t.GenericMethodReturningGenericType<string>("foo"));
            t.VerifyAllExpectations();
        }

        [Fact]
        public void CanMockGenericMethodWithGenericParam()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.GenericMethodWithGenericParam<string>("foo"))
                .Return("bar");

            Assert.Equal("bar", t.GenericMethodWithGenericParam("foo"));
            t.VerifyAllExpectations();
        }

        [Fact]
        public void CanMockGenericMethodInGenericClass()
        {
            GenericTestClass<string> t = MockRepository.Mock<GenericTestClass<string>>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t.Expect(x => x.GenericMethod<int>("foo"))
                .Return(42);

            Assert.Equal(42, t.GenericMethod<int>("foo"));
            t.VerifyAllExpectations();
        }

        [Fact]
        public void CanMockAppDomain()
        {
            AppDomain appDomain = MockRepository.Mock<AppDomain>();
            appDomain.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            appDomain.Expect(x => x.BaseDirectory)
                .Return("/home/user/ayende");

            Assert.Equal(appDomain.BaseDirectory, "/home/user/ayende");
            appDomain.VerifyAllExpectations();
        }

        [Fact]
        public void NotCallingExpectedMethodWillCauseVerificationError()
        {
            AppDomain appDomain = MockRepository.Mock<AppDomain>();
            appDomain.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            appDomain.Expect(x => x.BaseDirectory)
                .Return("/home/user/ayende");

            Assert.Throws<ExpectationViolationException>(
                () => appDomain.VerifyAllExpectations());
        }

        [Fact]
        public void CanMockMethodAcceptingTestClass()
        {
            TestClass t1 = MockRepository.Mock<TestClass>();
            t1.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            TestClass t2 = MockRepository.Mock<TestClass>();
            t2.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            t1.Expect(x => x.MethodAcceptingTestClass(t2));

            t1.MethodAcceptingTestClass(t2);
            t1.VerifyAllExpectations();
        }

        [Fact]
        // can't use ExpectedException since expected message is dynamic
        public void CanMockMethodAcceptingTestClass_WillErrorOnWrongParameter()
        {
            string t2Text = "@";
            string t3Text = "@";

            try
            {

                TestClass t1 = MockRepository.Mock<TestClass>();
                t1.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
                TestClass t2 = MockRepository.Mock<TestClass>();
                t2.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
                TestClass t3 = MockRepository.Mock<TestClass>();
                t3.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

                t2Text = t2.ToString();
                t3Text = t3.ToString();

                t1.Expect(x => x.MethodAcceptingTestClass(t2));

                t1.MethodAcceptingTestClass(t3);
                t1.VerifyExpectations(true);

                Assert.False(true, "Expected ExpectationViolationException");
            }
            catch (ExpectationViolationException ex)
            {
                string msg =
                    string.Format("TestClass.MethodAcceptingTestClass({0}); Expected #0, Actual #1.\r\n" +
                                  "TestClass.MethodAcceptingTestClass(equal to {1}); Expected #1, Actual #0.",
                                  t3Text, t2Text);

                Assert.Equal(msg, ex.Message);
            }
        }

        [Fact]
        public void StrictMockGetTypeReturnsMockedType()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            Assert.Same(typeof(TestClass), t.GetType());
        }

        [Fact]
        public void StrictMockGetHashCodeWorks()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            t.GetHashCode();
        }

        [Fact]
        public void StrictMockToStringReturnsDescription()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            int hashCode = t.GetHashCode();
            string toString = t.ToString();
            Assert.Equal(String.Format("RemotingMock_{0}<TestClass>", hashCode), toString);
        }

        [Fact]
        public void StrictMockEquality()
        {
            TestClass t = MockRepository.Mock<TestClass>();
            t.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            Assert.False(t.Equals(null));
            Assert.False(t.Equals(42));
            Assert.False(t.Equals("foo"));
            Assert.True(t.Equals(t));
        }
    }
}
