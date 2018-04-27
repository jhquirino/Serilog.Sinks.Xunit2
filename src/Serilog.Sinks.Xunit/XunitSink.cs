// -----------------------------------------------------------------------
//  <copyright file="XunitSink.cs" company="Jorge Alberto Hernández Quirino">
//  Copyright (c) Jorge Alberto Hernández Quirino 2018. All rights reserved.
//  </copyright>
//  <author>Jorge Alberto Hernández Quirino</author>
// -----------------------------------------------------------------------
using System;
using System.IO;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Xunit.Abstractions;

namespace Serilog.Sinks.Xunit
{
    /// <summary>
    /// Xunit (<see cref="ITestOutputHelper"/>) destination for log events.
    /// </summary>
    /// <seealso cref="ILogEventSink"/>
    /// <seealso cref="ITestOutputHelper"/>
    internal class XunitSink : ILogEventSink
    {
        /// <summary>
        /// The default helper to format log events in a textual representation.
        /// </summary>
        private static readonly ITextFormatter defaultFormatter = new MessageTemplateTextFormatter(XunitLoggerConfigurationExtensions.DefaultXunitOutputTemplate, null);

        /// <summary>
        /// The helper to provide Xunit test output.
        /// </summary>
        private readonly ITestOutputHelper outputHelper;

        /// <summary>
        /// The helper to format log events in a textual representation.
        /// </summary>
        private readonly ITextFormatter formatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="XunitSink"/> class.
        /// </summary>
        /// <param name="outputHelper">The helper to provide Xunit test output.</param>
        public XunitSink(ITestOutputHelper outputHelper) : this(outputHelper, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XunitSink"/> class.
        /// </summary>
        /// <param name="outputHelper">The helper to provide Xunit test output.</param>
        /// <param name="formatter">The helper to format log events in a textual representation.</param>
        public XunitSink(ITestOutputHelper outputHelper, ITextFormatter formatter)
        {
            this.outputHelper = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));
            this.formatter = formatter ?? defaultFormatter;
        }

        /// <summary>
        /// Emit the provided log event to the sink (<see cref="ITestOutputHelper"/>).
        /// </summary>
        /// <param name="logEvent">The log event to write.</param>
        public void Emit(LogEvent logEvent)
        {
            using (var writer = new StringWriter())
            {
                if (logEvent.Exception == null) writer.NewLine = string.Empty;
                formatter.Format(logEvent, writer);
                outputHelper.WriteLine(writer.ToString());
            }
        }
    }
}