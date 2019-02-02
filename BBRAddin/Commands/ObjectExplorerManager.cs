using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace BBRAddin.Commands
{
    public class ObjectExplorerManager
    {
        private readonly BaseCommandPackage _package;
        private HierarchyObject _tableMenu;
        private string _tableUrnPath = "Server/Database/Table";

        public ObjectExplorerManager(BaseCommandPackage package)
        {
            _package = package;
        }

        private IObjectExplorerService GetObjectExplorer()
        {
            return _package.GetServiceHelper(typeof(IObjectExplorerService)) as IObjectExplorerService;
        }

        public void SetObjectExplorerEventProvider()
        {
            var mi = GetType().GetMethod("Provider_SelectionChanged", BindingFlags.NonPublic | BindingFlags.Instance);
            var objectExplorer = GetObjectExplorer();
            if (objectExplorer == null) return;
            var t = Assembly.Load("Microsoft.SqlServer.Management.SqlStudio.Explorer").GetType("Microsoft.SqlServer.Management.SqlStudio.Explorer.ObjectExplorerService");

            objectExplorer.GetSelectedNodes(out int nodeCount, out INodeInformation[] nodes);

            var piContainer = t.GetProperty("Container", BindingFlags.Public | BindingFlags.Instance);
            var objectExplorerContainer = piContainer.GetValue(objectExplorer, null);
            var piContextService = objectExplorerContainer.GetType().GetProperty("Components", BindingFlags.Public | BindingFlags.Instance);
            var objectExplorerComponents = piContextService.GetValue(objectExplorerContainer, null) as ComponentCollection;
            object contextService = null;

            if (objectExplorerComponents != null)
                foreach (Component component in objectExplorerComponents)
                {
                    if (component.GetType().FullName.Contains("ContextService"))
                    {
                        contextService = component;
                        break;
                    }
                }
            if (contextService == null)
                throw new NullReferenceException("Can't find ObjectExplorer ContextService.");

            var piObjectExplorerContext = contextService.GetType().GetProperty("ObjectExplorerContext", BindingFlags.Public | BindingFlags.Instance);
            var objectExplorerContext = piObjectExplorerContext.GetValue(contextService, null);
            var ei = objectExplorerContext.GetType().GetEvent("CurrentContextChanged", BindingFlags.Public | BindingFlags.Instance);
            if (ei == null) return;
            var del = Delegate.CreateDelegate(ei.EventHandlerType, this, mi);
            ei.AddEventHandler(objectExplorerContext, del);
        }

        private void Provider_SelectionChanged(object sender, NodesChangedEventArgs args)
        {
            if (args.ChangedNodes.Count <= 0) return;
            var node = args.ChangedNodes[0];
            if (node == null) return;
            Debug.WriteLine(node.UrnPath);
            Debug.WriteLine(node.Name);
            Debug.WriteLine(node.Context);

            if (_tableMenu == null && _tableUrnPath == node.UrnPath)
            {
                _tableMenu = (HierarchyObject)node.GetService(typeof(IMenuHandler));
                var item = new TableMenuItem(_package);
                _tableMenu.AddChild(string.Empty, item);
            }
        }
    }
}
