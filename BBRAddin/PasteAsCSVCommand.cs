//------------------------------------------------------------------------------
// <copyright file="BaseCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using BBRAddin.Helpers;
using BBRAddin.Services;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace BBRAddin
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class PasteAsCSVCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("67bb7c7b-8c9f-4000-8df2-f9b77750493d");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        public static string CurrentTableName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private PasteAsCSVCommand(Package package)
        {
            this.package = package ?? throw new ArgumentNullException("package");

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static PasteAsCSVCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        public IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new PasteAsCSVCommand(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            var clipboardText = ClipboardService.GetText();
            if (string.IsNullOrWhiteSpace(clipboardText))
                return;

            var formatedText = TextHelper.GetFormattedText(clipboardText);
            if (string.IsNullOrWhiteSpace(formatedText))
                return;

            var dte = (this.package as BaseCommandPackage).GetServiceHelper(typeof(DTE)) as DTE;
            if (dte != null)
            {
                var doc = (TextDocument)dte.Application.ActiveDocument.Object(null);

                doc.Selection.Text = formatedText;

                //doc.EndPoint.CreateEditPoint().Insert(formatedText);
            }
        }
    }
}
