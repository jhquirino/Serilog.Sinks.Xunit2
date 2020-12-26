// -----------------------------------------------------------------------
//  <copyright file="LogTests.cs" company="Jorge Alberto Hernández Quirino">
//  Copyright (c) Jorge Alberto Hernández Quirino 2018. All rights reserved.
//  </copyright>
//  <author>Jorge Alberto Hernández Quirino</author>
// -----------------------------------------------------------------------
using System;
using Serilog.Configuration;
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

        [Fact]
        public void When_init_with_null_sink_config_Then_throw_exception()
        {
            LoggerSinkConfiguration sinkConfig = null;
            Assert.Throws<ArgumentNullException>(() =>
                sinkConfig.Xunit(output)
            );
        }

        [Fact]
        public void When_init_with_null_sink_config2_Then_throw_exception()
        {
            LoggerSinkConfiguration sinkConfig = null;
            Assert.Throws<ArgumentNullException>(() =>
                sinkConfig.Xunit(outputHelper: output, formatter: null)
            );
        }

        [Fact]
        public void When_init_with_null_output_Then_throw_exception()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new LoggerConfiguration().WriteTo.Xunit(null)
            );
        }

        [Fact]
        public void When_init_with_null_output2_Then_throw_exception()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new LoggerConfiguration().WriteTo.Xunit(outputHelper: null, formatter: null)
            );
        }

        [Fact]
        public void When_init_with_null_template_Then_throw_exception()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new LoggerConfiguration().WriteTo.Xunit(output, outputTemplate: null)
            );
        }

        [Fact]
        public void When_init_with_null_formatter_Then_throw_exception()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new LoggerConfiguration().WriteTo.Xunit(output, formatter: null)
            );
        }

        [Fact]
        public void When_instance_with_null_output_Then_throw_exception()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new XunitSink(null)
            );
        }

        [Fact]
        public void When_instance_with_null_formatter_Then_is_initialized()
        {
            var sink = new XunitSink(output);
            Assert.NotNull(sink);
        }
    }
}