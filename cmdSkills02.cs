using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.Exceptions;
using System.Windows.Controls;


namespace RAB_Bootcamp_Projects
{
    [Transaction(TransactionMode.Manual)]
    public class cmdSkills02 : IExternalCommand
    {
        public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Your Module 02 Skills code goes here
            // Delete the TaskDialog below and add your code
            // TaskDialog.Show("Module 02 Skills", "Got Here!");

            // 1A Pick single element
            Reference pickRef = uidoc.Selection.PickObject( Autodesk.Revit.UI.Selection.ObjectType.Element, "Select Element" );
            Element pickElem = doc.GetElement( pickRef );

            // 1b Pick multiple elements
            List<Element> pickList = uidoc.Selection.PickElementsByRectangle( "Select Elements" ).ToList();

            TaskDialog.Show( "Test", $"I selected {pickList.Count} elements" );

            // 2a filter selected elements
            List<CurveElement> allCurves = new List<CurveElement>();
            foreach ( Element elem in pickList )
            {
                if ( elem is CurveElement )
                {
                    allCurves.Add( elem as CurveElement );
                }
            }

            // 2b Filter selected elements for model curves

            List<CurveElement> modelCurves = new List<CurveElement>();
            foreach ( Element elem2 in pickList )
            {
                if ( elem2 is CurveElement )
                {
                    //CurveElement curveElem = elem2 as CurveElement;
                    CurveElement curveElem = ( CurveElement ) elem2;

                    if ( curveElem.CurveElementType == CurveElementType.ModelCurve )
                    {
                        modelCurves.Add( curveElem );
                    }
                }
            }

            // 3 Curve data
            foreach ( CurveElement currentCurve in modelCurves )
            {
                Curve curve = currentCurve.GeometryCurve;
                XYZ startPoint = curve.GetEndPoint( 0 );
                XYZ endPoint = curve.GetEndPoint( 1 );

                GraphicsStyle curStyle = currentCurve.LineStyle as GraphicsStyle;

                Debug.Print( curStyle.Name );

            }

            // Transaction t = new Transaction( doc );
            // t.Start( "Create Wall" );

            // Cretae Transaction with using statement
            using ( Transaction t = new Transaction( doc ) )

            {
                t.Start( "Create Wall" );
                // 4a create a wall
                Level newLevel = Level.Create( doc, 20 );

                CurveElement curveElem = modelCurves[0];
                Curve curCurve = curveElem.GeometryCurve;

                Curve curCurve2 = modelCurves[1].GeometryCurve;

                Wall newWall = Wall.Create( doc, curCurve, newLevel.Id, false );

                // 4b Create wall with wall type

                FilteredElementCollector wallTypes = new FilteredElementCollector( doc );
                //wallTypes.OfClass(typeof( WallType ) );
                wallTypes.OfCategory( BuiltInCategory.OST_Walls );
                wallTypes.WhereElementIsElementType();

                Wall newWall2 = Wall.Create( doc, curCurve2, wallTypes.FirstElementId(), newLevel.Id, 20, 0, false, false );

                // 5 get system types
                FilteredElementCollector systemCollector = new FilteredElementCollector( doc );
                systemCollector.OfClass( typeof( MEPSystemType ) );

                // 6 get duct type
                MEPSystemType ductSystem = GetSystemTypeByName(doc, "Supply Air" );
                /* made redundant by method
                foreach ( MEPSystemType systemType in systemCollector )
                {
                    if ( systemType.Name == "Supply Air" )
                    {
                        ductSystem = systemType;
                        break;
                    }

                }
                */


                // 8 get duct type

                FilteredElementCollector ductCollector = new FilteredElementCollector( doc );
                ductCollector.OfClass( typeof( DuctType ) );

                // 9 create duct
                Curve curCurve3 = modelCurves[2].GeometryCurve;
                Duct newDuct = Duct.Create( doc, ductSystem.Id, ductCollector.FirstElementId(), newLevel.Id, curCurve3.GetEndPoint( 0 ), curCurve3.GetEndPoint( 1 ) );

                // 10 get pipe system type
                MEPSystemType pipeSystem = GetSystemTypeByName(doc, "Domestic Hot Water" );
                /* made redundant by method
                foreach ( MEPSystemType systemType in systemCollector )
                {
                    if ( systemType.Name == "Domestic Hot Water" )
                    {
                        pipeSystem = systemType;
                        break;
                    }

                }
                */

                // 11 get pipe type
                FilteredElementCollector pipeCollector = new FilteredElementCollector( doc );
                pipeCollector.OfClass( typeof( PipeType ) );

                // 12 create pipe
                Curve curCurve4 = modelCurves[3].GeometryCurve;
                Pipe newPipe = Pipe.Create( doc, pipeSystem.Id, pipeCollector.FirstElementId(), newLevel.Id, curCurve4.GetEndPoint( 0 ), curCurve4.GetEndPoint( 1 ) );

                // 13 switch statement
                int numberValue = 5;
                string numberAsString = "";
                switch ( numberValue )
                {
                    case 1:
                        numberAsString = "One";
                        break;
                    case 2:
                        numberAsString = "Two";
                        break;
                    case 3:
                        numberAsString = "Three";
                        break;
                    case 4:
                        numberAsString = "Four";
                        break;
                    case 5:
                        numberAsString = "Five";
                        break;
                    default:
                        numberAsString = "Not a valid number";
                        break;
                }




                t.Commit();

            }




            return Result.Succeeded;
        }

        internal string MyFirstMethod()
        {
            return "This is my first method";

        }

        internal void MySecondMetod()
        {
            Debug.Print("My second Method" );

        }

        internal string MyThirdMethod(string input)
        {
            string returnString = $"This is my third method: {input}";
            return returnString;
        }

        internal MEPSystemType GetSystemTypeByName(Document doc, string name)
        {  
            //  get all system types
            FilteredElementCollector systemCollector = new FilteredElementCollector( doc );
            systemCollector.OfClass( typeof( MEPSystemType ) );

            // 6 get duct type
            foreach ( MEPSystemType systemType in systemCollector )
            {
                if ( systemType.Name == name )
                {
                    return systemType;
                }

            }
            return null;
        }
    }
 

}

