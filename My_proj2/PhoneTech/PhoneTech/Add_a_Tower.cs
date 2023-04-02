using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Desktop.AddIns;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;

namespace PhoneTech
{
    public class Add_a_Tower : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        public Add_a_Tower()
        {
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }


        protected override void OnMouseUp(MouseEventArgs arg)
        {
            var x = arg.X;
            var y = arg.Y;

            IMxDocument mxDoc=(IMxDocument)ArcMap.Application.Document;
            IFeatureLayer pFLayer =(IFeatureLayer) mxDoc.ActiveView.FocusMap.Layer[0];
            IDataset fdaset = (IDataset)pFLayer.FeatureClass;
            IWorkspace workspace=fdaset.Workspace;

            IPoint pPoint = mxDoc.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            if (pPoint == null)
                return;

            TowerManager towermanager = new TowerManager(workspace);
            //Tower tower=towermanager.GetTowerByID("T04");
            Tower tower = towermanager.GetNearestTower(pPoint);
            if (tower==null)
            {
                MessageBox.Show("No pole found");
                return;
            }
            MessageBox.Show("ID:"+tower.ID,Environment.NewLine+"Type:" +tower.TowerType+ Environment.NewLine+"BAND:"+ tower.NetworkBand);



        }
    }

}
