using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace PhoneTech
{
    class TowerManager
    {
        private IWorkspace _workspace;
        public TowerManager(IWorkspace workspace)
        {
            _workspace = workspace;
        }

        public Tower GetTowerByID(String toweId)
        {
            IFeatureWorkspace workspace = (IFeatureWorkspace)_workspace;
            IFeatureClass featureclass=workspace.OpenFeatureClass("Tower");
            IQueryFilter qFilter = new QueryFilter();
            qFilter.WhereClause = "TOWERID = '"+toweId+"'";
            IFeatureCursor featurecursor=featureclass.Search(qFilter,true);
            IFeature feature=featurecursor.NextFeature();
            if (feature == null)
                return null;

            return GetTower(feature);

        }

        private Tower GetTower(IFeature feature)
        {
            Tower tower = new Tower();
            tower.ID = feature.Value[feature.Fields.FindField("TOWERID")];
            tower.NetworkBand = feature.Value[feature.Fields.FindField("NETWORKBAND")];
            tower.TowerType = feature.Value[feature.Fields.FindField("TOWERTYPE")];
            tower.TowerLocation =(IPoint) feature.Shape;
            return tower;
        }

        public Tower GetNearestTower(IPoint point)
        {
            ITopologicalOperator topo = (ITopologicalOperator)point;
            IGeometry buffer=topo.Buffer(12);


            ISpatialFilter spatialFilter = new SpatialFilter();
            spatialFilter.Geometry = buffer;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            IFeatureWorkspace workspace = (IFeatureWorkspace)_workspace;
            IFeatureClass featureclass = workspace.OpenFeatureClass("Tower");

            IFeatureCursor sCursor= featureclass.Search(spatialFilter,true);
            IFeature feature=sCursor.NextFeature();
            if (feature == null)
                return null;

            return GetTower(feature);
          



        }



    }
}
