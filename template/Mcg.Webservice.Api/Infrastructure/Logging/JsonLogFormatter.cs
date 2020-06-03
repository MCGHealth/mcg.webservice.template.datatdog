using System;
using System.IO;
using System.Linq;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;
using Serilog.Parsing;

namespace Mcg.Webservice.Api.Infrastructure.Logging
{
	/// <summary>
	/// A custom log formatter for MCG apps.
	/// </summary>
	public class JsonLogFormatter : ITextFormatter
	{
		private JsonValueFormatter ValueFormatter => new JsonValueFormatter(typeTagName: "$type");

		/// <summary>
		/// Format the log event into the output. Subsequent events will be newline-delimited.
		/// </summary>
		/// <param name="logEvent">The event to format.</param>
		/// <param name="output">The output.</param>
		public void Format(LogEvent logEvent, TextWriter output)
		{
			FormatEvent(logEvent, output, ValueFormatter);
			output.WriteLine();
		}

		/// <summary>
		/// Format the log event into the output.
		/// </summary>
		/// <param name="logEvent">The event to format.</param>
		/// <param name="output">The output.</param>
		/// <param name="valueFormatter">A value formatter for <see cref="LogEventPropertyValue"/>s on the event.</param>
		public static void FormatEvent(LogEvent logEvent, TextWriter output, JsonValueFormatter valueFormatter)
		{
			if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
			if (output == null) throw new ArgumentNullException(nameof(output));
			if (valueFormatter == null) throw new ArgumentNullException(nameof(valueFormatter));

			output.Write("{\"timestamp\":\"");
			output.Write(logEvent.Timestamp.UtcDateTime.ToString("O"));
			output.Write("\"");

			var tokensWithFormat = logEvent.MessageTemplate.Tokens
				.OfType<PropertyToken>()
				.Where(pt => pt.Format != null);

			output.Write(",\"level\":\"");

			//--> limiting the size of the level value to help reduce the overall size of the log
			//    entry without making it too terse.
			output.Write(logEvent.Level.ToString().ToUpper().Substring(0, 3));
			output.Write('\"');

			foreach (var property in logEvent.Properties)
			{
				var name = property.Key.ToLower();
				if (name.Length > 0 && name[0] == '@')
				{
					// Escape first '@' by doubling
					name = '@' + name;
				}

				output.Write(',');
				JsonValueFormatter.WriteQuotedJsonString(name, output);
				output.Write(':');
				valueFormatter.Format(property.Value, output);
			}

			output.Write('}');
		}
	}
}
