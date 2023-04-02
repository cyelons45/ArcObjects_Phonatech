using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace Phonetech
{
    public class TowerManager
    {
        private IWorkspace _workspace;
        public TowerManager(IWorkspace pWorkspace)
        {
            _workspace = pWorkspace;
        }


         public Tower GetTowerByID(string towerid)
        {
            IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)_workspace;
            IFeatureClass fcTower= pFeatureWorkspace.OpenFeatureClass("Tower");


            IQueryFilter queryFilter = new QueryFilter();
            queryFilter.WhereClause = "TOWERID='"+ towerid + "'";
            IFeatureCursor pFcursor = fcTower.Search(queryFilter, true);
            IFeature pTowerFeature=  pFcursor.NextFeature();

            if (pTowerFeature == null)
                return null;

            return GetTower(pTowerFeature);


        }

        public Tower GetTower(IFeature pTowerFeature)
        {
            Tower tower = new Tower();
            tower.ID = pTowerFeature.get_Value(pTowerFeature.Fields.FindField("TOWERID"));
            tower.NetworkBand = pTowerFeature.get_Value(pTowerFeature.Fields.FindField("NETWORKBAND"));
            tower.TowerType = pTowerFeature.get_Value(pTowerFeature.Fields.FindField("TOWERTYPE"));

            return tower;
        }


        public Tower GetNearestTower(IPoint pPoint)
        {
            ITopologicalOperator pTopo =(ITopologicalOperator) pPoint;
            IGeometry bufferedPoint=(IGeometry)pTopo.Buffer(12.0);
            ISpatialFilter spatialFilter = new SpatialFilter();
            spatialFilter.Geometry = bufferedPoint;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)_workspace;
            IFeatureClass fcTower = pFeatureWorkspace.OpenFeatureClass("Tower");


            IFeatureCursor pFCursor=fcTower.Search(spatialFilter, true);
            IFeature pTowerFeature=pFCursor.NextFeature();
            if (pTowerFeature == null)
                return null;

            return GetTower(pTowerFeature);

        }


    }
}
