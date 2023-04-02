using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneTech
{
    
    class Tower
    {
        public String ID { get; set; }
        public String TowerType { get; set; }
        public String NetworkBand { get; set; }
        public Double TowerCost { get; set; }
        public Double TowerCoverage { get; set; }
        public Double TowerHeight { get; set; }
        public Double TowerBaseArea { get; set; }
        public IPoint TowerLocation { get; set; }
    }


}
