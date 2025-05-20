using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Windows.Media.Imaging; // need this for Bitmap functions


namespace RAB_Bootcamp_Projects
{
    public class AppSkills : IExternalApplication
    {
        public Result OnStartup( UIControlledApplication application )
        {
            // Create tab
            string tabname = "My Tab";
            application.CreateRibbonTab( tabname );

            // Create tab and panel - safer version
            try
            {
                application.CreateRibbonTab( tabname );
            }
            catch ( Exception error )
            {
                Debug.Print( "Tab already exists, using existing panel" );
                Debug.Print( error.Message );
                // Tab already exists
            }

            // Create panel
            string panelName1 = "Test Panel 1";
            string panelName2 = "Test Panel 2";
            string panelName3 = "Test Panel 3";

            RibbonPanel panel = application.CreateRibbonPanel( tabname, panelName1 );
            RibbonPanel panel2 = application.CreateRibbonPanel( panelName2 );

            // RibbonPanel panel3 = application.CreateRibbonPanel( "Architecture", panelName3 );
            // can't use built-in panels

            // get existing panel
            List<RibbonPanel> panelList = application.GetRibbonPanels();
            List<RibbonPanel> panelList2 = application.GetRibbonPanels( tabname );

            // Get Panel Method - safe method
            RibbonPanel panel4 = CreateGetPanel(application, tabname, panelName1 );

            // button data object
            PushButtonData buttonData1 = new PushButtonData(
                "button1",
                "Command 1",
                Assembly.GetExecutingAssembly().Location,
                "RAB_Bootcamp_Projects.cmdSkills04" );


            PushButtonData buttonData2 = new PushButtonData(
                "button2",
                "Button \r Command 2",
                 Assembly.GetExecutingAssembly().Location,
                 "RAB_Bootcamp_Projects.cmdSkills04" );


            // add tooltips

            buttonData1.ToolTip = "This is a tooltip for Command 1";
            buttonData2.ToolTip = "This is a tooltip for Command 2";

            // add images 
            buttonData1.LargeImage = ConvertToImageSource( Properties.Resources.Green_32 );
            buttonData1.Image = ConvertToImageSource( Properties.Resources.Green_16 );
            buttonData2.LargeImage = ConvertToImageSource( Properties.Resources.Blue_32 );
            buttonData2.Image = ConvertToImageSource( Properties.Resources.Blue_16 );

            // add buttons to panel
            // PushButton button1 = panel.AddItem( buttonData1 ) as PushButton;
            // PushButton button2 = panel.AddItem( buttonData2 ) as PushButton;

            // ad stackable buttons
            // panel.AddStackedItems(buttonData1, buttonData2 );

            // split button
            //SplitButtonData splitButtonData = new SplitButtonData( "SplitButton",
            //    "Split\rButton" );
            //SplitButton splitButton = panel.AddItem(splitButtonData ) as SplitButton;
            //splitButton.AddPushButton( buttonData1 );
            //splitButton.AddPushButton( buttonData2 );

            // add pulldown button
            PulldownButtonData pulldownButtonData = new PulldownButtonData( "PulldownButton",
                "Pulldown\rButton" );
            pulldownButtonData.LargeImage = ConvertToImageSource( Properties.Resources.Green_32 );
            PulldownButton pulldownButton = panel.AddItem( pulldownButtonData ) as PulldownButton;
            pulldownButton.AddPushButton( buttonData1 );
            pulldownButton.AddPushButton( buttonData2 );


            // other items
            panel.AddSeparator();
            panel.AddSlideOut();
            // this code is evaluated in order, which creates panel structure











            return Result.Succeeded;
        }

        private RibbonPanel CreateGetPanel( UIControlledApplication application, string tabname, string panelName1 )
        {
            // look for panel in tab
            foreach (RibbonPanel panel in application.GetRibbonPanels( tabname ) )
            {
                if ( panel.Name == panelName1 )
                {
                    return panel;
                }
            }
            // panel not found, create it
            // RibbonPanel returnPanel = application.CreateRibbonPanel( tabname, panelName1 );
            // return returnPanel;
            // code below does same thing, but in one step

            return application.CreateRibbonPanel( tabname, panelName1 );
        }

        public Result OnShutdown( UIControlledApplication application )
        {
            return Result.Succeeded;
        }

        public BitmapImage ConvertToImageSource( byte[] imageData )
        {
            using ( MemoryStream stream = new MemoryStream( imageData ) )
            {
                stream.Position = 0; // Reset the stream position to the beginning
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                return image;
            }
        }

    }
}
