using System;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;
using EnvDTE;
using BBRAddin.Model;
using System.Text;
using MenuItem = System.Windows.Controls.MenuItem;
using Microsoft.VisualStudio.Shell;
using BBRAddin.Services;

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

            var scriptValidInActiveWindow = new ToolStripMenuItem("SELECT TOP 100 Valid (active window)");
            scriptValidInActiveWindow.Click += ScriptValidActiveWindow_Click;

            var scriptById = new ToolStripMenuItem("SELECT valid by Id");
            scriptById.Click += ScriptValidById_Click;

            var scriptByIdInActiveWindow = new ToolStripMenuItem("SELECT valid by Id (active window)");
            scriptByIdInActiveWindow.Click += ScriptValidByIdActiveWindow_Click;

            item.DropDownItems.Add(scriptValid);
            item.DropDownItems.Add(scriptById);
            item.DropDownItems.Add(scriptValidInActiveWindow);
            item.DropDownItems.Add(scriptByIdInActiveWindow);

            return new ToolStripItem[] { item };
        }

        private void ScriptValid_Click(object sender, EventArgs e)
        {
            ScriptValidBase_Click(sender, e, true);
        }

        private void ScriptValidActiveWindow_Click(object sender, EventArgs e)
        {
            ScriptValidBase_Click(sender, e, false);
        }

        private void ScriptValidBase_Click(object sender, EventArgs e, bool newWindow)
        {
            try
            {
                var menuItem = GetMenuItem();

                if (menuItem == null)
                    return;

                if (newWindow)
                {
                    ScriptFactory.Instance.CreateNewBlankScript(ScriptType.Sql);
                }
                var dte = _package.GetServiceHelper(typeof(DTE)) as DTE;
                if (dte != null)
                {
                    var doc = (TextDocument)dte.Application.ActiveDocument.Object(null);

                    string query = QueryService.GetSelectQuery(menuItem.Tag.ToString(), null);

                    doc.EndPoint.CreateEditPoint().Insert(query);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ScriptValidById_Click(object sender, EventArgs e)
        {
            ScriptValidByIdBase_Click(sender, e, true);
        }

        private void ScriptValidByIdActiveWindow_Click(object sender, EventArgs e)
        {
            ScriptValidByIdBase_Click(sender, e, false);
        }

        private void ScriptValidByIdBase_Click(object sender, EventArgs e, bool newWindow)
        {
            try
            {
                var menuItem = GetMenuItem();

                if (menuItem == null)
                    return;

                if (newWindow)
                {
                    ScriptFactory.Instance.CreateNewBlankScript(ScriptType.Sql);
                }
                
                var dte = _package.GetServiceHelper(typeof(DTE)) as DTE;
                if (dte != null)
                {
                    var doc = (TextDocument)dte.Application.ActiveDocument.Object(null);

                    var clipBoardText = ClipboardService.GetText();
                    var clipBoardIsGuid = Guid.TryParse(clipBoardText, out Guid id);

                    string query = QueryService.GetSelectQuery(menuItem.Tag.ToString(), clipBoardIsGuid ? id.ToString() : string.Empty);

                    doc.EndPoint.CreateEditPoint().Insert(query);
                }
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
