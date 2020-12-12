using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

#nullable disable

namespace ClosestCity.Models
{
    public partial class WorldcitiesGeometry
    {
        public long? Id { get; set; }
        public string City { get; set; }
        public string CityAscii { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string Country { get; set; }
        public string Iso2 { get; set; }
        public string Iso3 { get; set; }
        public string AdminName { get; set; }
        public string Capital { get; set; }
        public double? Population { get; set; }
        public Geometry Geom { get; set; }
    }
}
