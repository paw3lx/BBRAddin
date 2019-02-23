using EnvDTE;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BBRAddin.Windows
{
    /// <summary>
    /// Interaction logic for QueryControl.xaml.
    /// </summary>
    [ProvideToolboxControl("BBRAddin.Windows.QueryControl", true)]
    public partial class QueryControl : UserControl
    {
        private BaseCommandPackage _package;
        public QueryControl()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            string id = ObjectId.Text;

            ScriptFactory.Instance.CreateNewBlankScript(ScriptType.Sql);
            var dte = BaseCommand.Instance.ServiceProvider.GetService(typeof(DTE)) as DTE;
            if (dte != null)
            {
                var doc = (TextDocument)dte.Application.ActiveDocument.Object(null);

                var query = Services.QueryService.GetSelectQuery(BaseCommand.CurrentTableName, id);

                doc.EndPoint.CreateEditPoint().Insert(query);
            }

            IVsUIShell vsUIShell = (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));
            Guid guid = typeof(QueryWindow).GUID;
            IVsWindowFrame windowFrame;
            int result = vsUIShell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fFindFirst, ref guid, out windowFrame);   // Find MyToolWindow

            windowFrame.CloseFrame((int)__FRAMECLOSE.FRAMECLOSE_NoSave);
        }
    }
}
