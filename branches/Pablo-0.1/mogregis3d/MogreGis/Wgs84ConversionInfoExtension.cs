using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjNet.CoordinateSystems;

namespace MogreGis
{
    static class Wgs84ConversionInfoExtension
    {
        private  const double SEC_TO_RAD = 4.84813681109535993589914102357e-6;

        public static double[] GetAffineTransform(this Wgs84ConversionInfo aux)
        {
            double RS = 1 + aux.Ppm * 0.000001;
            return new double[7] { RS, aux.Ex * SEC_TO_RAD * RS, aux.Ey * SEC_TO_RAD * RS, aux.Ez * SEC_TO_RAD * RS, aux.Dx, aux.Dy, aux.Dz };
            
        }
    }
}
