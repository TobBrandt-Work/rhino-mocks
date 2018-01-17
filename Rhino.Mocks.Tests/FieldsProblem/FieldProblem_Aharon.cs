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
using System.Runtime.InteropServices;
using Rhino.Mocks.Exceptions;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_Aharon
    {
        [Fact]
        public void CanCreateInterfaceWithGuid()
        {
            IUniqueID bridgeRemote = MockRepository.Mock<IUniqueID>();
            bridgeRemote.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            Assert.NotNull(bridgeRemote);
        }


        [Fact]
        public void MockingDataset()
        {
            MyDataSet controller = MockRepository.Mock<MyDataSet>();
            controller.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);
            Assert.NotNull(controller);
        }

        [Fact]
        public void PassingMockToMock_WhenErrorOccurs()
        {
            Accepter accepter = MockRepository.Mock<Accepter>();
            accepter.SetUnexpectedBehavior(UnexpectedCallBehaviors.BaseOrDefault);

            accepter.Accept(accepter);
            Assert.Throws<ExpectationViolationException>(
                () => accepter.VerifyExpectations(true));
        }
    }

    public abstract class Accepter
    {
        public abstract void Accept(Accepter demo);

        public override string ToString()
        {
            return base.ToString();
        }
    }

    [Guid("AFCF8240-61AF-4089-BD98-3CD4D719EDBA")]
    public interface IUniqueID
    {
    }

    public class MyDataSet : System.Data.DataSet, IClearable
    {
    }

    public interface IClearable
    {
        void Clear();
    }
}