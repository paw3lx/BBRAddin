//------------------------------------------------------------------------------
// <copyright file="BaseCommandPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using System.Timers;
using BBRAddin.Commands;
using BBRAddin.Windows;

namespace BBRAddin
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(QueryWindow))]
    [Guid(BaseCommandPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class BaseCommandPackage : Package
    {
        /// <summary>
        /// BaseCommandPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "39cee19f-759c-4a67-8b95-eeab65a3540a";

        private ObjectExplorerManager _objectExplorerManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand"/> class.
        /// </summary>
        public BaseCommandPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            BaseCommand.Initialize(this);
            base.Initialize();

            AddSkipLoading();

            _objectExplorerManager = new ObjectExplorerManager(this);
            _objectExplorerManager.SetObjectExplorerEventProvider();
        }

        private void AddSkipLoading()
        {
            var timer = new Timer(2000);
            timer.Elapsed += (sender, args) =>
            {
                timer.Stop();

                var myPackage = UserRegistryRoot.CreateSubKey(@"Packages\{" + PackageGuidString + "}");
                myPackage?.SetValue("SkipLoading", 1);
            };
            timer.Start();
        }

        #endregion

        public object GetServiceHelper(Type type)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return GetService(type);
        }

        public void ShowQueryWindow(object sender, EventArgs e, string tableName)
        {
            var window = FindToolWindow(typeof(QueryWindow), 0, true);
            if (window?.Frame == null)
            {
                return;
            }
            BaseCommand.CurrentTableName = tableName;
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}
