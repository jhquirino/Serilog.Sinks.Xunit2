// -----------------------------------------------------------------------
//  <copyright file="LogTests.cs" company="Jorge Alberto Hernández Quirino">
//  Copyright (c) Jorge Alberto Hernández Quirino 2018. All rights reserved.
//  </copyright>
//  <author>Jorge Alberto Hernández Quirino</author>
// -----------------------------------------------------------------------
using System;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Serilog.Sinks.Xunit.UnitTests
{
    [Trait("Serilog.Sinks.Xunit", "UnitTests")]
    public class LogTests
    {
        private readonly TestOutputHelper output;

        public LogTests(ITestOutputHelper output)
        {
            this.output = output as TestOutputHelper;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Xunit(this.output)
                .WriteTo.Console()
                .CreateLogger();
        }

        [Fact]
        public void When_log_message_Then_message_is_in_output()
        {
            var message = "Simple message";
            Assert.Empty(output.Output);
            Log.Information(message);
            Assert.Contains(message, output.Output);
        }

        [Fact]
        public void When_log_exception_Then_exception_is_in_output()
        {
            var message = "Exception message.";
            var exceptionMessage = "Test exception thrown!";
            var exception = new InvalidOperationException(exceptionMessage);
            Assert.Empty(output.Output);
            Log.Error(exception, message);
            Assert.Contains(message, output.Output);
            Assert.Contains(exception.GetType().FullName, output.Output);
            Assert.Contains(exceptionMessage, output.Output);
        }
    }
}