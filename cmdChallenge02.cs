using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;

namespace RAB_Bootcamp_Projects
{
    [Transaction(TransactionMode.Manual)]
    public class cmdChallenge02 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Your Module 02 Challenge code goes here
            // Delete the TaskDialog below and add your code
            // TaskDialog.Show("Module 02 Challenge", "Coming Soon!");

            // Pick multiple elements
            List<Element> pickList = uidoc.Selection.PickElementsByRectangle( "Select Elements" ).ToList();

            // TaskDialog.Show( "Test", $"I selected {pickList.Count} elements" );

            // Filter selected elements for model curves

            using ( Transaction t = new Transaction( doc ) )
            {
                t.Start( "Objects from lines" );

                List<CurveElement> modelCurves = new List<CurveElement>();
                foreach ( Element elem in pickList )
                {
                    if ( elem is CurveElement )
                    {
                        //CurveElement curveElem = elem2 as CurveElement;
                        CurveElement curveElem = ( CurveElement ) elem;

                        if ( curveElem.CurveElementType == CurveElementType.ModelCurve )
                        {
                            modelCurves.Add( curveElem );
                        }
                    }
                }

                // Curve data
                List<CurveElement> glazCurves = new List<CurveElement>();
                List<CurveElement> wallCurves = new List<CurveElement>();
                List<CurveElement> ductCurves = new List<CurveElement>();
                List<CurveElement> pipeCurves = new List<CurveElement>();

                foreach ( CurveElement currentCurve in modelCurves )
                {
                    // Curve curve = currentCurve.GeometryCurve;
                    // XYZ startPoint = curve.GetEndPoint( 0 ); invalid for circles / ellipses
                    // XYZ endPoint = curve.GetEndPoint( 1 );

                    GraphicsStyle curStyle = currentCurve.LineStyle as GraphicsStyle;
                    switch ( curStyle.Name )
                    {
                        case "A-GLAZ":
                            glazCurves.Add( currentCurve );
                            break;

                        case "A-WALL":
                            wallCurves.Add( currentCurve );
                            break;

                        case "M-DUCT":
                            ductCurves.Add( currentCurve );
                            break;

                        case "P-PIPE":
                            pipeCurves.Add( currentCurve );
                            break;

                        default:
                            break;
                    }


                }

                FilteredElementCollector levelList = new FilteredElementCollector( doc );
                levelList.OfCategory( BuiltInCategory.OST_Levels );
                levelList.WhereElementIsNotElementType();

                Level targetLevel = null;
                foreach ( Level nameLevel in levelList )
                {
                    if ( nameLevel.Name == "Level 1" )
                    {
                        targetLevel = nameLevel;
                        break;
                    }
                }

                WallType glazType = GetWallTypeByName( doc, "Storefront" );
                WallType genericType = GetWallTypeByName( doc, "Generic - 8\"" );


                foreach ( CurveElement eleCurve in glazCurves )
                {
                    Curve curve = eleCurve.GeometryCurve;
                    Wall newWall = Wall.Create( doc, curve, glazType.Id, targetLevel.Id, 10.0, 0.0, false, false );
                }

                foreach ( CurveElement eleCurve in wallCurves )
                {
                    Curve curve = eleCurve.GeometryCurve;
                    Wall newWall = Wall.Create( doc, curve, genericType.Id, targetLevel.Id, 10.0, 0.0, false, false );
                }

                MEPSystemType ductSystem = GetSystemTypeByName( doc, "Supply Air" );
             
                //  get duct type

                FilteredElementCollector ductCollector = new FilteredElementCollector( doc );
                ductCollector.OfClass( typeof( DuctType ) );

                //  create duct

                foreach ( CurveElement eleCurve in ductCurves )
                {
                    Curve curve = eleCurve.GeometryCurve;
                    Duct newDuct = Duct.Create( doc, ductSystem.Id, ductCollector.FirstElementId(), targetLevel.Id, curve.GetEndPoint( 0 ), curve.GetEndPoint( 1 ) );
                }

                // creating pipes

                MEPSystemType pipeSystem = GetSystemTypeByName( doc, "Domestic Hot Water" );

                FilteredElementCollector pipeCollector = new FilteredElementCollector( doc );
                pipeCollector.OfClass( typeof( PipeType ) );

                foreach ( CurveElement eleCurve in pipeCurves )
                {
                    Curve curve = eleCurve.GeometryCurve;
                    Pipe newPipe = Pipe.Create( doc, pipeSystem.Id, pipeCollector.FirstElementId(), targetLevel.Id, curve.GetEndPoint( 0 ), curve.GetEndPoint( 1 ) );
                }

                t.Commit();
            }
            return Result.Succeeded;
        }
        internal MEPSystemType GetSystemTypeByName( Document doc, string name )
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

        internal WallType GetWallTypeByName( Document doc, string name )
        {
            //  get all wall types
            FilteredElementCollector wallCollector = new FilteredElementCollector( doc );
            wallCollector.OfClass( typeof( WallType ) );
            // 6 get duct type
            foreach ( WallType wallType in wallCollector )
            {
                if ( wallType.Name == name )
                {
                    return wallType;
                }
            }
            return null;
        }
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnChallenge02";
            string buttonTitle = "Module\r02";

            Common.ButtonDataClass myButtonData = new Common.ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Module02,
                Properties.Resources.Module02,
                "Module 02 Challenge");

            return myButtonData.Data;
        }
    }

}
