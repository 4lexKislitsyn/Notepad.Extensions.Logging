using Notepad.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging
{
    public class NotepadLoggerOptions
    {
        /// <summary>
        /// Name of notepad window.
        /// </summary>
        public string WindowName { get; set; }
        /// <summary>
        /// Name of document. For Notepad it works only in EN language.
        /// </summary>
        public string DocumentName { get; set; }
        /// <summary>
        /// Application to search.
        /// </summary>
        internal WindowKind Application { get; set; }
    }
}
