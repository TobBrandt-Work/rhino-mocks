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
    public class CallOriginalMethodTests
    {
        [Fact]
        public void CallOriginalMethodOnPropGetAndSet()
        {
            MockingClassesTests.DemoClass demo = MockRepository.Partial<MockingClassesTests.DemoClass>();
            demo.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            demo.Expect(x => x.Prop)
                .Repeat.Any()
                .CallOriginalMethod();

            demo.Expect(x => x.Prop = Arg<int>.Is.Anything)
                .CallOriginalMethod();

            for (int i = 0; i < 10; i++)
            {
                demo.Prop = i;
                Assert.Equal(i, demo.Prop);
            }

            demo.VerifyAllExpectations();
        }

        [Fact]
        public void CantCallOriginalMethodOnInterface()
        {
            IDemo demo = MockRepository.Mock<IDemo>();
            demo.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            Assert.Throws<InvalidOperationException>(
                () => demo.Expect(x => x.ReturnIntNoArgs())
                        .CallOriginalMethod());
        }

        [Fact]
        public void CantCallOriginalMethodOnAbstractMethod()
        {
            MockingClassesTests.AbstractDemo demo = MockRepository.Partial<MockingClassesTests.AbstractDemo>();
            demo.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            Assert.Throws<InvalidOperationException>(
                () => demo.Expect(x => x.Six())
                        .CallOriginalMethod());
        }
    }
}
