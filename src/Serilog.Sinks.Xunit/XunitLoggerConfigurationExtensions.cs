// -----------------------------------------------------------------------
//  <copyright file="XunitLoggerConfigurationExtensions.cs" company="Jorge Alberto Hernández Quirino">
//  Copyright (c) Jorge Alberto Hernández Quirino 2018. All rights reserved.
//  </copyright>
//  <author>Jorge Alberto Hernández Quirino</author>
// -----------------------------------------------------------------------
using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.Xunit;
using Xunit.Abstractions;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.Xunit() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class XunitLoggerConfigurationExtensions
    {
        internal const string DefaultXunitOutputTemplate = "{Timestamp:O} {Level} - {Message}{NewLine}{Exception}";

        /// <summary>
        /// Writes log events to <see cref="ITestOutputHelper"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="outputHelper">Helper to provide Xunit test output.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime.</param>
        /// <param name="outputTemplate">A message template describing the format used to write to the sink, the default is <code>"{Timestamp:O} {Level} - {Message}{NewLine}{Exception}"</code>.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration Xunit(
            this LoggerSinkConfiguration sinkConfiguration,
            ITestOutputHelper outputHelper,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultXunitOutputTemplate,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (outputHelper == null) throw new ArgumentNullException(nameof(outputHelper));
            if (outputTemplate == null) throw new ArgumentNullException(nameof(outputTemplate));

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            return sinkConfiguration.Xunit(outputHelper, formatter, restrictedToMinimumLevel, levelSwitch);
        }

        /// <summary>
        /// Writes log events to <see cref="ITestOutputHelper"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="outputHelper">Helper to provide Xunit test output.</param>
        /// <param name="formatter">Controls the rendering of log events into text, for example to log JSON. To control plain text formatting, use the overload that accepts an output template.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration Xunit(
            this LoggerSinkConfiguration sinkConfiguration,
            ITestOutputHelper outputHelper,
            ITextFormatter formatter,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (outputHelper == null) throw new ArgumentNullException(nameof(outputHelper));
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            return sinkConfiguration.Sink(new XunitSink(outputHelper, formatter), restrictedToMinimumLevel, levelSwitch);
        }
    }
}