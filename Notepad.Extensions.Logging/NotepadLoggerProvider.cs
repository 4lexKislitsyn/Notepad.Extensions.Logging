using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;

namespace Notepad.Extensions.Logging
{
    [ProviderAlias("Notepad")]
    class NotepadLoggerProvider : ILoggerProvider
    {
        NotepadLoggerProvider()
        {
            var poolProvider = new DefaultObjectPoolProvider();
            stringBuilderPool = poolProvider.CreateStringBuilderPool();
            windowFinder = new WindowFinder();
        }

        public NotepadLoggerProvider(IOptionsMonitor<NotepadLoggerOptions> options) : this()
        {
            // Filter would be applied on LoggerFactory level
            optionsReloadToken = options.OnChange(ReloadLoggerOptions);
            ReloadLoggerOptions(options.CurrentValue);
        }

        public NotepadLoggerProvider(NotepadLoggerOptions options) : this()
        {
            this.options = options;
        }
        readonly ObjectPool<StringBuilder> stringBuilderPool;
        readonly IWindowFinder windowFinder;
        readonly IDisposable optionsReloadToken;
        NotepadLoggerOptions options;
        string searchingWindowName;
        
        public ILogger CreateLogger(string categoryName) => new NotepadLogger(stringBuilderPool, windowFinder, categoryName, searchingWindowName);

        public void Dispose()
        {
            optionsReloadToken?.Dispose();
        }

        void ReloadLoggerOptions(NotepadLoggerOptions options)
        {
            this.options = options;
            if (!string.IsNullOrWhiteSpace(options.WindowName))
            {
                searchingWindowName = options.WindowName;
            }
            else if (!string.IsNullOrWhiteSpace(options.DocumentName))
            {
                switch (options.Application)
                {
                    case WindowKind.NotepadPlusPlus:
                        searchingWindowName = $"{options.DocumentName} - Notepad++";
                        break;
                    case WindowKind.Notepad:
                        searchingWindowName = $"{options.DocumentName} - Notepad";
                        break;
                }
            }
        }
    }
}
