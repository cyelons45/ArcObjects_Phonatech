using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Desktop.AddIns;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;



namespace AddTowerTool
{
    public class Tower : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        public Tower()
        {
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }

        protected override void OnMouseUp(MouseEventArgs arg)
        {
            int x = arg.X;
            int y = arg.Y;

            IMxDocument mxDoc=(IMxDocument)ArcMap.Application.Document;

            IScreenDisplay screenDisplay = (IScreenDisplay)mxDoc.ActiveView.ScreenDisplay;
            IPoint pPoint = screenDisplay.DisplayTransformation.ToMapPoint(x, y);
            MessageBox.Show("X: " + pPoint.X, "Y: " + pPoint.Y);

        }
    }

}


//public ESRI.ArcGIS.Geometry.IPoint GetMapCoordinatesFromScreenCoordinates
//    (ESRI.ArcGIS.Geometry.IPoint screenPoint, ESRI.ArcGIS.Carto.IActiveView
//    activeView)
//{

//    if (screenPoint == null || screenPoint.IsEmpty || activeView == null)
//    {
//        return null;
//    }

//    ESRI.ArcGIS.Display.IScreenDisplay screenDisplay = activeView.ScreenDisplay;
//    ESRI.ArcGIS.Display.IDisplayTransformation displayTransformation =
//        screenDisplay.DisplayTransformation;

//    return displayTransformation.ToMapPoint((System.Int32)screenPoint.X,
//        (System.Int32)screenPoint.Y); // Explicit cast.