using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    public static class Matrix4Extension
    {
        /// <summary>
        /// Extensión de Mogre.Matrix4 para poder hacer la misma funcion que hacia
        /// osg::makeTranslate.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Mogre.Matrix4 makeTranslate(this Mogre.Matrix4 matrix, double x, double y, double z)
        {
            matrix = new Mogre.Matrix4((float)1, (float)0, (float)0, (float)0,
                                        (float)0, (float)1, (float)0, (float)0,
                                        (float)0, (float)0, (float)1, (float)0,
                                        (float)x, (float)y, (float)z, (float)1);
            return matrix;
        }
    }
}