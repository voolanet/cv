using System;
using System.Reflection;
using Autodesk.Revit.UI;
using MyRevitTools.Commands;

namespace MyRevitTools
{
    /// <summary>
    /// Revit start hone par chalta hai: Ribbon tab, panel aur buttons banata hai.
    /// </summary>
    public class App : IExternalApplication
    {
        private const string TabName = "My Tools";

        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                application.CreateRibbonTab(TabName);
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                // Tab pehle se bana hua hai - koi baat nahi.
            }

            RibbonPanel panel = application.CreateRibbonPanel(TabName, "Inspect");
            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            AddButton<ElementInfoCommand>(panel, assemblyPath,
                "Element\nInfo",
                "Kisi element ko select karke uski details aur parameters dekho.");

            AddButton<CountElementsCommand>(panel, assemblyPath,
                "Count\nElements",
                "Active view me har category ke kitne elements hain, ye dikhata hai.");

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application) => Result.Succeeded;

        private static void AddButton<TCommand>(RibbonPanel panel, string assemblyPath, string text, string tooltip)
            where TCommand : IExternalCommand
        {
            Type type = typeof(TCommand);
            var data = new PushButtonData(type.Name, text, assemblyPath, type.FullName)
            {
                ToolTip = tooltip
            };
            panel.AddItem(data);
        }
    }
}
