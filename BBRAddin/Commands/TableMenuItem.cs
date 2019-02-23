using System;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;
using EnvDTE;
using BBRAddin.Model;
using System.Text;
using MenuItem = System.Windows.Controls.MenuItem;
using Microsoft.VisualStudio.Shell;

namespace BBRAddin.Commands
{
    public class TableMenuItem : ToolsMenuItemBase, IWinformsMenuHandler
    {
        private readonly BaseCommandPackage _package;

        public TableMenuItem(BaseCommandPackage package)
        {
            _package = package;
            Text = "Script table...";
        }

        protected override void Invoke()
        {
        }

        public override object Clone() => new TableMenuItem(_package);

        public ToolStripItem[] GetMenuItems()
        {
            var item = new ToolStripMenuItem("BBR Scripts");

            var scriptValid = new ToolStripMenuItem("SELECT TOP 100 Valid");
            scriptValid.Click += ScriptValid_Click;

            var scriptById = new ToolStripMenuItem("SELECT valid by Id");
            scriptById.Click += ScriptValidById_Click;

            item.DropDownItems.Add(scriptValid);
            item.DropDownItems.Add(scriptById);

            return new ToolStripItem[] { item };
        }

        private void ScriptValid_Click(object sender, EventArgs e)
        {
            try
            {
                var menuItem = GetMenuItem();

                if (menuItem == null)
                    return;

                ScriptFactory.Instance.CreateNewBlankScript(ScriptType.Sql);
                var dte = _package.GetServiceHelper(typeof(DTE)) as DTE;
                if (dte != null)
                {
                    var doc = (TextDocument)dte.Application.ActiveDocument.Object(null);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"SELECT TOP 100 * FROM {menuItem.Tag}");
                    sb.AppendLine("WHERE RegistreringTil IS NULL AND VirkningTil IS NULL");

                    doc.EndPoint.CreateEditPoint().Insert(sb.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ScriptValidById_Click(object sender, EventArgs e)
        {
            try
            {
                var menuItem = GetMenuItem();

                if (menuItem == null)
                    return;

                //ask for Id
                Guid id = Guid.Empty;
                if (Clipboard.ContainsText(TextDataFormat.Text))
                {
                    string clipboardText = Clipboard.GetText(TextDataFormat.Text);
                    Guid.TryParse(clipboardText, out id);
                }

                _package.ShowQueryWindow(this, e, (string)menuItem.Tag);

            }
            catch (Exception ex)
            {
            }
        }

        private MenuItem GetMenuItem()
        {
            var parent = Parent?.Parent;
            if (parent?.Connection == null) return null;

            var menuItem = new MenuItem
            {
                CommandParameter = new MenuCommandParameters
                {
                    MenuItemType = MenuType.Table,
                    Name = parent.InvariantName
                },
                Tag = Parent.InvariantName //Table name
            };
            return menuItem;
        }
    }
}
