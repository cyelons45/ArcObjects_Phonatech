using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Desktop.AddIns;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;

namespace Phonetech
{
    public class Add_Tower : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        public Add_Tower()
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

            

            IMxDocument pMxdoc=(IMxDocument)ArcMap.Application.Document;
            IFeatureLayer pfeaturelayer= (IFeatureLayer)pMxdoc.ActiveView.FocusMap.Layer[0];
            IDataset pDS = (IDataset)pfeaturelayer.FeatureClass;
            TowerManager tm = new TowerManager(pDS.Workspace);

            IPoint pPoint = pMxdoc.ActivatedView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            Tower t = tm.GetNearestTower(pPoint);
            if (t==null)
            {
                MessageBox.Show("No Tower found");
                return;
            }
            MessageBox.Show("Tower id "+t.ID+ Environment.NewLine+"NetworkBand "+ t.NetworkBand+ Environment.NewLine+ "Tower type " +t.TowerType);



        }
    }

}
