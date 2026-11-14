using System.Linq;
using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace MyRevitTools.Commands
{
    /// <summary>
    /// Active view ke saare elements ko category ke hisaab se ginta hai.
    /// FilteredElementCollector ka basic example.
    /// </summary>
    [Transaction(TransactionMode.ReadOnly)]
    public class CountElementsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            if (uidoc == null)
            {
                message = "Koi document khula nahi hai.";
                return Result.Failed;
            }

            Document doc = uidoc.Document;
            View view = doc.ActiveView;

            var counts = new FilteredElementCollector(doc, view.Id)
                .WhereElementIsNotElementType()
                .Where(e => e.Category != null)
                .GroupBy(e => e.Category.Name)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            var sb = new StringBuilder();
            foreach (var row in counts)
            {
                sb.AppendLine($"{row.Category}: {row.Count}");
            }

            var dialog = new TaskDialog("Count Elements")
            {
                MainInstruction = $"View \"{view.Name}\" me {counts.Sum(c => c.Count)} elements",
                MainContent = sb.Length > 0 ? sb.ToString() : "Is view me koi element nahi mila."
            };
            dialog.Show();

            return Result.Succeeded;
        }
    }
}
