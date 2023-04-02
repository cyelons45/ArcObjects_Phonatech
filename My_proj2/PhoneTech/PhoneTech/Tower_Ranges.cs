using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace PhoneTech
{
    public class Tower_Ranges : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public Tower_Ranges()
        {
        }

        protected override void OnClick()
        {

            IMxDocument mxDoc = (IMxDocument)ArcMap.Application.Document;
            IFeatureLayer pFLayer = (IFeatureLayer)mxDoc.ActiveView.FocusMap.Layer[0];
            IDataset fdaset = (IDataset)pFLayer.FeatureClass;
            IWorkspace workspace = fdaset.Workspace;

            TowerManager towermanager = new TowerManager(workspace);
            Tower tower=towermanager.GetTowerByID("T04");

            ITopologicalOperator pTopo = (ITopologicalOperator)tower.TowerLocation;

            int towerRange = 100;
            IPolygon ThreeBars =(IPolygon)pTopo.Buffer(towerRange *1/3);
            IPolygon TwoBars = (IPolygon)pTopo.Buffer(towerRange *2/3);
            ITopologicalOperator pIntTopo2Bar = (ITopologicalOperator)TwoBars;

            IPolygon TwoBarsDonut = (IPolygon)pIntTopo2Bar.SymmetricDifference(ThreeBars);

            IPolygon OneBar = (IPolygon)pTopo.Buffer(towerRange *3/3);

            ITopologicalOperator pIntTopo1Bar = (ITopologicalOperator)OneBar;
            IPolygon OneBarsDonut = (IPolygon)pIntTopo1Bar.SymmetricDifference(TwoBars);

            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)fdaset.Workspace;
            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();

            IFeatureWorkspace pworkspace = (IFeatureWorkspace)workspaceEdit;
            IFeatureClass pTowerRangeFC = pworkspace.OpenFeatureClass("TowerRange");
            IFeature pFeature=pTowerRangeFC.CreateFeature();

            pFeature.set_Value(pFeature.Fields.FindField("TOWERID"), "T04");
            pFeature.set_Value(pFeature.Fields.FindField("RANGE"), 3);
            pFeature.Shape = ThreeBars;
            pFeature.Store();



            IFeature pFeature2Bar = pTowerRangeFC.CreateFeature();

            pFeature2Bar.set_Value(pFeature.Fields.FindField("TOWERID"), "T04");
            pFeature2Bar.set_Value(pFeature.Fields.FindField("RANGE"), 2);
            pFeature2Bar.Shape = TwoBarsDonut;
            pFeature2Bar.Store();



            IFeature pFeature1Bar = pTowerRangeFC.CreateFeature();

            pFeature1Bar.set_Value(pFeature.Fields.FindField("TOWERID"), "T04");
            pFeature1Bar.set_Value(pFeature.Fields.FindField("RANGE"), 1);
            pFeature1Bar.Shape = OneBarsDonut;
            pFeature1Bar.Store();

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

        }

        protected override void OnUpdate()
        {
        }
    }
}
