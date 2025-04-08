namespace RAB_Bootcamp_Projects
{
    [Transaction(TransactionMode.Manual)]
    public class cmdSkills01 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            string text1 = "some text";
            string text2 = "more text";

            string text3 = text1 + " " + text2;

            int number1 = 10;
            double number2 = 20.5;

            double number3 = number1 + number2;

            double number4 = number1 - number2;

            double number5 = number4 / number3;

            double number6 = number5 * number4;

            double meters = 4;
            double metersToFeet = meters * 3.28084;

            double mm = 3500;
            double mmToFeet = mm / 304.8;

            double mmToFeet2 = (mm/1000) * 3.28084;

            // modulo

            double remainder1 = 100 % 10; // returns 0
            double remainder2 = 100 % 9; // returns 1

            number6++;
            number6--;

            // logical operators

            if (number6 > 10)
            {
                // do some stuff

            }

            if (number5 == 100)
            {
                // do something
            }
            else
            {
                // do something if false
            }

            if (number6 > 10)
            {
                // do if true, if so, returns from here
            }
            else if (number6 == 8)
            {
                // do something
            }
            else
            {
                // third thing
            }

            // compound conditional

            if (number6 >10 && number5 == 100)
            {
                // do stuff
            }

            if (number6 > 10 || number5 == 100)
            {
                //do stuff
            }

            // create a list

            List<string> list1 = new List<string>();

            // add items

            list1.Add(text1);
            list1.Add(text2);
            list1.Add("this is some text");

            List<int> list2 = new List<int> { 1, 2, 3, 4, 5, };
            List<string> list3 = new List<string> { "one", "two", "three", "four", "five" };

            // for loops

            foreach (string currentString in list1)
            {
                // do something with currentString
            }

            int letterCount = 0;
            foreach(string currentString in list1)
            {
                letterCount += currentString.Length;
            }

            for (int i = 0; i <= 10; i++)
            {
                // do something
            }

            int numberCount = 0;
            int counter = 100;

            for (int i = 0; i <= counter; i++)
            {
                numberCount += i;
            }



            // Your Module 01 Skills code goes here
            // Delete the TaskDialog below and add your code
            TaskDialog.Show("Module 01 Skills", "Got Here!");

            // create transaction

            Transaction t = new Transaction(doc);
            t.Start("Some stuff in Revit");
            // make change
            // create floor level
            Level newLevel = Level.Create(doc, 10);
            newLevel.Name = "My Level";

            // create filtered element collector to get view family type

            FilteredElementCollector collector1 = new FilteredElementCollector(doc);
            collector1.OfClass(typeof(ViewFamilyType));

            ViewFamilyType floorPlanVFT = null;
            foreach (ViewFamilyType curVFT in collector1)
            {
                if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                {
                    floorPlanVFT = curVFT;
                    break;
                }
            }

            // create floor plan

            ViewPlan newFloorPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);

            newFloorPlan.Name = "My Floor Plan";

            // create ceiling plan

            ViewFamilyType ceilingPlanVFT = null;
            foreach (ViewFamilyType curVFT in collector1)
            {
                if (curVFT.ViewFamily == ViewFamily.CeilingPlan)
                {
                    ceilingPlanVFT = curVFT;
                    break;
                }
            }

            ViewPlan newCeilingPlan = ViewPlan.Create(doc, ceilingPlanVFT.Id, newLevel.Id);

            newCeilingPlan.Name = "My Ceiling Plan";

            // find titleblocks

            FilteredElementCollector collector2 = new FilteredElementCollector(doc);
            collector2.OfCategory(BuiltInCategory.OST_TitleBlocks);
            collector2.WhereElementIsElementType();

            // create new sheet, using first titleblock found
            ViewSheet newSheet = ViewSheet.Create(doc, collector2.FirstElementId());
            newSheet.Name = "My new sheet";
            newSheet.SheetNumber = "A101";

            // create a viewport
            // first create a point
            XYZ insPoint = new XYZ();
            XYZ insPoint2 = new XYZ(1, 0.5, 0);

            Viewport newViewport = Viewport.Create(doc, newSheet.Id, newFloorPlan.Id, insPoint);




            t.Commit();
            t.Dispose();

            return Result.Succeeded;
        }
    }

}
