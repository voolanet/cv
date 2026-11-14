using System.Linq;
using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using MyRevitTools.Utils;

namespace MyRevitTools.Commands
{
    /// <summary>
    /// User ek element pick karta hai; uski category, type aur parameters dikhaye jaate hain.
    /// Sirf padhta hai - model change nahi karta, isliye ReadOnly.
    /// </summary>
    [Transaction(TransactionMode.ReadOnly)]
    public class ElementInfoCommand : IExternalCommand
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

            Reference picked;
            try
            {
                picked = uidoc.Selection.PickObject(ObjectType.Element, "Ek element select karo");
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled; // User ne Esc dabaya
            }

            Element element = doc.GetElement(picked);
            Element type = doc.GetElement(element.GetTypeId());

            var sb = new StringBuilder();
            sb.AppendLine($"Name:      {element.Name}");
            sb.AppendLine($"Id:        {element.Id.GetIdValue()}");
            sb.AppendLine($"Category:  {element.Category?.Name ?? "-"}");
            sb.AppendLine($"Type:      {type?.Name ?? "-"}");
            sb.AppendLine();
            sb.AppendLine("Parameters:");

            foreach (Parameter p in element.Parameters.Cast<Parameter>()
                         .Where(p => p.HasValue)
                         .OrderBy(p => p.Definition.Name))
            {
                string value = p.StorageType == StorageType.String
                    ? p.AsString()
                    : p.AsValueString();
                sb.AppendLine($"  {p.Definition.Name} = {value}");
            }

            var dialog = new TaskDialog("Element Info")
            {
                MainInstruction = $"{element.Category?.Name}: {element.Name}",
                MainContent = sb.ToString()
            };
            dialog.Show();

            return Result.Succeeded;
        }
    }
}
