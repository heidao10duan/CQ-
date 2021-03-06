// <copyright file="TestExcelToDtTest.cs">Copyright ©  2018</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;
using TestExcelToDt;

namespace TestExcelToDt.Tests
{
    /// <summary>此类包含 TestExcelToDt 的参数化单元测试</summary>
    [PexClass(typeof(global::TestExcelToDt.TestExcelToDt))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class TestExcelToDtTest
    {
        /// <summary>测试 TestMethod() 的存根</summary>
        [PexMethod]
        public void TestMethodTest([PexAssumeUnderTest]global::TestExcelToDt.TestExcelToDt target)
        {
            target.TestMethod();
            // TODO: 将断言添加到 方法 TestExcelToDtTest.TestMethodTest(TestExcelToDt)
        }
    }
}
