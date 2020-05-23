using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using Notepad.Extensions.Logging;
using System;

namespace Microsoft.Extensions.Logging
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddNotepad(this ILoggingBuilder builder)
            => AddProvider(builder, WindowKind.Notepad);

        public static ILoggingBuilder AddNotepad(this ILoggingBuilder builder, Action<NotepadLoggerOptions> configure)
            => AddProvider(builder, WindowKind.Notepad, configure);

        public static ILoggingBuilder AddNotepadPlusPlus(this ILoggingBuilder builder)
            => AddProvider(builder, WindowKind.NotepadPlusPlus);

        public static ILoggingBuilder AddNotepadPlusPlus(this ILoggingBuilder builder, Action<NotepadLoggerOptions> configure)
            => AddProvider(builder, WindowKind.NotepadPlusPlus, configure);

        static ILoggingBuilder AddProvider(this ILoggingBuilder builder, WindowKind kind, Action<NotepadLoggerOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }
            AddProvider(builder, kind);
            builder.Services.Configure(configure);
            return builder;
        }

        static ILoggingBuilder AddProvider(ILoggingBuilder builder, WindowKind kind)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, NotepadLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<NotepadLoggerOptions, NotepadLoggerProvider>(builder.Services);

            if (kind != default)
            {
                builder.Services.Configure<NotepadLoggerOptions>(opt => opt.Application = kind);
            }

            return builder;
        }
    }
}
