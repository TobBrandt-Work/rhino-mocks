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


using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldsProblem_Royston
    {
        public FieldsProblem_Royston()
        {
        }

        public interface IDuplicateType<T>
        {
            int Property { get; }
        }

        [Fact]
        public void DuplicateTypeTest()
        {
            // Let's just create two mocks of the same type, based on
            // an array type parameter.

            // This should not blow up.

            IDuplicateType<object[]> mock1 = MockRepository.Mock<IDuplicateType<object[]>>();
            mock1.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            IDuplicateType<object[]> mock2 = MockRepository.Mock<IDuplicateType<object[]>>();
            mock2.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            mock1.VerifyExpectations(true);
            mock2.VerifyExpectations(true);
        }

        [Fact]
        public void TestVirtualEntrypoint()
        {
            IIntf1 i1 = CreateAndConfigureMock();

            i1.VirtualGo();
            i1.VerifyAllExpectations();
        }

        [Fact]
        public void TestNonVirtualEntrypoint()
        {
            IIntf1 i1 = CreateAndConfigureMock();

            i1.NonVirtualGo();
            i1.VerifyAllExpectations();
        }

        private IIntf1 CreateAndConfigureMock()
        {
            IIntf1 i1 = (IIntf1) MockRepository.Partial<Cls1>();
            i1.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            i1.Expect(x => x.A());
            i1.Expect(x => x.B());
            i1.Expect(x => x.A());

            return i1;
        }

        public interface IIntf1
        {
            void A();
            void B();
            void C();
            void VirtualGo();
            void NonVirtualGo();
        }

        public abstract class Cls1 : IIntf1
        {
            public abstract void A();
            public abstract void B();
            public abstract void C();

            public virtual void VirtualGo()
            {
                A();
                B();
                A();
            }

            public void NonVirtualGo()
            {
                A();
                B();
                A();
            }
        }
    }
}
