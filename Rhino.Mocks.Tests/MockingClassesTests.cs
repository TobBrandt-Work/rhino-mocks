﻿#region license
// Copyright (c) 2005 - 2007 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
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
using Xunit;

namespace Rhino.Mocks.Tests
{

    public class MockingClassesTests
    {
        private DemoClass demoClass;

        public MockingClassesTests()
        {
            demoClass = MockRepository.Partial<DemoClass>();
            demoClass.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
        }

        [Fact]
        public void MockVirtualCall()
        {
            demoClass.Expect(x => x.Two())
                .Return(3);

            Assert.Equal(3, demoClass.Two());

            demoClass.VerifyAllExpectations();
        }

        [Fact]
        public void MockClassWithParametrizedCtor()
        {
            ParametrizedCtor pc = MockRepository.Partial<ParametrizedCtor>(3, "Hello");
            pc.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            Assert.Equal(3, pc.Int);
            Assert.Equal("Hello", pc.String);

            pc.Expect(x => x.Add(0, 1))
                .Return(10);

            Assert.Equal(10, pc.Add(0, 1));
            pc.VerifyAllExpectations();
        }

        [Fact]
        public void MockClassWithOverloadedCtor()
        {
            OverLoadedCtor oc = MockRepository.Partial<OverLoadedCtor>(1);
            oc.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            OverLoadCtorExercise(oc, 1, null);

            oc = MockRepository.Partial<OverLoadedCtor>("Hello");
            oc.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            OverLoadCtorExercise(oc, 0, "Hello");

            oc = MockRepository.Partial<OverLoadedCtor>(33, "Hello");
            oc.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            OverLoadCtorExercise(oc, 33, "Hello");
        }

        [Fact]
        public void BadParamsToCtor()
        {
            try
            {
                MockRepository.Partial<OverLoadedCtor>("Ayende", 55);

                Assert.False(true, "The above call should have failed");
            }
            catch (ArgumentException argumentException)
            {
                Assert.Contains(
                    "Can not instantiate proxy of class: Rhino.Mocks.Tests.MockingClassesTests+OverLoadedCtor",
                    argumentException.Message);
            }
        }

        [Fact]
        public void ToStringMocked()
        {
            if (demoClass.ToString() == "")
            {
                Assert.False(true, "ToString() of a mocked object is empty");
            }
        }

        [Fact]
        public void GetTypeMocked()
        {
            Assert.True(typeof(DemoClass).IsAssignableFrom(demoClass.GetType()));
        }

        [Fact]
        public void GetHashCodeMocked()
        {
            Assert.Equal(demoClass.GetHashCode(), demoClass.GetHashCode());
        }

        [Fact]
        public void EqualsMocked()
        {
            Assert.True(demoClass.Equals(demoClass));
        }

        private void OverLoadCtorExercise(OverLoadedCtor oc, int i, string s)
        {
            Assert.Equal(i, oc.I);
            Assert.Equal(s, oc.S);

            oc.Expect(x => x.Concat("Ayende", "Rahien"))
                .Return("Hello, World");

            Assert.Equal("Hello, World", oc.Concat("Ayende", "Rahien"));
            oc.VerifyAllExpectations();
        }

        public class DemoClass : IDisposable
        {
            int _prop;
            public bool disposableCalled;

            public virtual int Prop
            {
                get { return _prop; }
                set { _prop = value; }
            }

            public int One()
            {
                return 1;
            }

            public virtual int Two()
            {
                return 2;
            }

            void IDisposable.Dispose()
            {
                disposableCalled = true;
            }
        }

        public abstract class AbstractDemo
        {
            public virtual int Five()
            {
                return 0;
            }

            public abstract string Six();
        }

        public class ParametrizedCtor
        {
            private int i;
            private string s;

            public ParametrizedCtor(int i, string s)
            {
                this.i = i;
                this.s = s;
            }

            public int Int
            {
                get { return i; }
                set { i = value; }
            }

            public string String
            {
                get { return s; }
                set { s = value; }
            }

            public virtual int Add(int i1, int i2)
            {
                return i1 + i2;
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        public class OverLoadedCtor
        {
            private int i;
            private string s;

            public OverLoadedCtor(int i)
            {
                this.i = i;
            }

            public OverLoadedCtor(string s)
            {
                this.s = s;
            }

            public OverLoadedCtor(int i, string s)
            {
                this.i = i;
                this.s = s;
            }

            public int I
            {
                get { return i; }
            }

            public string S
            {
                get { return s; }
            }

            public virtual string Concat(string s1, string s2)
            {
                return s1 + s2;
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}