using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace BBRAddin.Windows
{
    [Guid("6708bce8-391c-43d9-a698-47de4e6b4f6a")]
    public class QueryWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryControl"/> class.
        /// </summary>
        public QueryWindow() : base(null)
        {
            this.Caption = "Query params";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new QueryControl();
        }
    }
}
