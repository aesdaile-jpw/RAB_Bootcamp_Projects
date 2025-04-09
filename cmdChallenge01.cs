namespace RAB_Bootcamp_Projects
{
    [Transaction(TransactionMode.Manual)]
    public class cmdChallenge01 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Your Module 01 Challenge code goes here
            // Delete the TaskDialog below and add your code
            // TaskDialog.Show("Module 01 Challenge", "Coming Soon!");

            int height = 250; // decimal feet (yuck) as int because we are using a loop counter and simple addition
            double floorHeightmm = 4572; // in mm because I'm metric

            double elevationFeet = 0;
            double floorHeightFeet = floorHeightmm / 304.8; // meteric to imperial conversion for practice

            Transaction t = new Transaction(doc, "Create Levels");
            t.Start("FizzBuzz Levels");

            // Get Floor Plan View Family Type
            FilteredElementCollector collector1 = new FilteredElementCollector(doc);
            collector1.OfClass(typeof(ViewFamilyType));
            ViewFamilyType floorPlanVFT = null;
            foreach (ViewFamilyType vft in collector1)
            {
                if (vft.ViewFamily == ViewFamily.FloorPlan)
                {
                    floorPlanVFT = vft;
                    break;
                }
            }

            // Get Veiling Plan View Family Type
            ViewFamilyType ceilingPlanVFT = null;
            foreach (ViewFamilyType vft in collector1)
            {
                if (vft.ViewFamily == ViewFamily.CeilingPlan)
                {
                    ceilingPlanVFT = vft;
                    break; // break because we don't need to keep scanning after a match
                }
            }

            FilteredElementCollector collector2 = new FilteredElementCollector(doc);
            collector2.OfCategory(BuiltInCategory.OST_TitleBlocks);
            collector2.WhereElementIsElementType();

            // set counters
            int fizzCount = 0;
            int buzzCount = 0;
            int fizzBuzzCount = 0;


            // Level creation loop
            for (int i = 1; i <= height; i++)
            {
                Level newLevel = Level.Create(doc, elevationFeet);
                newLevel.Name = "Level " + i.ToString();
                if (i % 3 == 0 && i % 5 == 0)
                { // FizzBuzz creates sheets, creates plans, puts plans on sheets
                    ViewSheet newSheet = ViewSheet.Create(doc, collector2.FirstElementId());
                    fizzBuzzCount++;
                    newSheet.Name = "FizzBuzz_" + fizzBuzzCount.ToString();
                    newSheet.SheetNumber = fizzBuzzCount.ToString();

                    ViewPlan newFloorPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
                    newFloorPlan.Name = "FizzBuzz_" + fizzBuzzCount.ToString();

                    XYZ insPooint = new XYZ(1.5, 1.25, 0);
                    Viewport newViewport = Viewport.Create(doc, newSheet.Id, newFloorPlan.Id, insPooint);
                }
                else if (i % 3 == 0)
                { // Fizz creates floor plans
                    ViewPlan newFloorPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
                    fizzCount++;
                    newFloorPlan.Name = "Fizz_" + fizzCount.ToString();
                }
                else if (i % 5 == 0)
                { // Buzz creates ceiling plans
                    ViewPlan newCeilingPlan = ViewPlan.Create(doc, ceilingPlanVFT.Id, newLevel.Id);
                    buzzCount++;
                    newCeilingPlan.Name = "Buzz_" + buzzCount.ToString();
                }

                // Increment elevation for next level
                elevationFeet = elevationFeet + floorHeightFeet;


            }



            t.Commit();
            t.Dispose();


            return Result.Succeeded;
        }
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnChallenge01";
            string buttonTitle = "Module\r01";

            Common.ButtonDataClass myButtonData = new Common.ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Module01,
                Properties.Resources.Module01,
                "Module 01 Challenge");

            return myButtonData.Data;
        }
    }

}
