using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems;

namespace MogreGis
{
    class MatrixTransform : MathTransform
    {
        protected IMathTransform _inverse;
		private Wgs84ConversionInfo _ToWgs94;
		double[] v;
        int dimension = 4;

		private bool _isInverse = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixTranform"/> class.
        /// </summary>
        /// <param name="towgs84"></param>
        public MatrixTransform(Wgs84ConversionInfo towgs84) : this(towgs84,false)
		{
		}

		private MatrixTransform(Wgs84ConversionInfo towgs84, bool isInverse)
		{
			_ToWgs94 = towgs84;
			v = _ToWgs94.GetAffineTransform();
			_isInverse = isInverse;
		}

        public MatrixTransform(double[,] matrix)
            //: this(towgs84, false)
        {
            //copiar matrix del parametro a la matrix v
            v = new double[matrix.Length];
            foreach (double d in matrix)
            {
                int i = 0;
                v[i] = d;
                i++;
            }
        }

        public MatrixTransform(double[] matrix)
        //: this(towgs84, false)
        {
            //copiar matrix del parametro a la matrix v
            v = new double[matrix.Length];
            foreach (double d in matrix)
            {
                int i = 0;
                v[i] = d;
                i++;
            }
        }

        public MatrixTransform(int dim, Mogre.Matrix4 reference_frame)
            : this(reference_frame)
        //: this(towgs84, false)
        {
            this.dimension = dim;
        }
        public MatrixTransform(Mogre.Matrix4 reference_frame)
        //: this(towgs84, false)
        {
            //copiar reference a la matrix v
            v = new double[16];
            v[0] = reference_frame.m00;
            v[1] = reference_frame.m01;
            v[2] = reference_frame.m02;
            v[3] = reference_frame.m03;
            v[4] = reference_frame.m10;
            v[5] = reference_frame.m11;
            v[6] = reference_frame.m12;
            v[7] = reference_frame.m13;
            v[8] = reference_frame.m20;
            v[9] = reference_frame.m21;
            v[10] = reference_frame.m22;
            v[11] = reference_frame.m23;
            v[12] = reference_frame.m30;
            v[13] = reference_frame.m31;
            v[14] = reference_frame.m32;
            v[15] = reference_frame.m33;

        }
        /// <summary>
        /// Gets a Well-Known text representation of this object.
        /// </summary>
        /// <value></value>
		public override string WKT
		{
			get { throw new NotImplementedException(); }
		}

        /// <summary>
        /// Gets an XML representation of this object.
        /// </summary>
        /// <value></value>
		public override string XML
		{
			get { throw new NotImplementedException(); }
		}

        /// <summary>
        /// Creates the inverse transform of this object.
        /// </summary>
        /// <returns></returns>
        /// <remarks>This method may fail if the transform is not one to one. However, all cartographic projections should succeed.</remarks>
		public override IMathTransform Inverse()
		{
			if (_inverse == null)
				_inverse = new MatrixTransform(_ToWgs94,!_isInverse);
			return _inverse;
		}


        private double[] Apply(double[] p)
		{
            if (dimension == 2)
                return Apply_2(p);
            else
            {
                double[] tmp = new double[] {
				v[0] * p[0] - v[3] * p[1] + v[2] * p[2] + v[4],
				v[3] * p[0] + v[0] * p[1] - v[1] * p[2] + v[5],
			   -v[2] * p[0] + v[1] * p[1] + v[0] * p[2] + v[6], };
                return tmp;
            }
		}

        private double[] Apply_2(double[] p)
        {
            double[] tmp = new double[] {
				v[0] * p[0],
				v[1] * p[1] };
            return tmp;
        }

        private double[] ApplyInverted(double[] p)
		{
            return new double[] {
				v[0] * p[0] + v[3] * p[1] - v[2] * p[2] - v[4],
			   -v[3] * p[0] + v[0] * p[1] + v[1] * p[2] - v[5],
			    v[2] * p[0] - v[1] * p[1] + v[0] * p[2] - v[6], };
		}

        /// <summary>
        /// Transforms a coordinate point. The passed parameter point should not be modified.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override double[] Transform(double[] point)
		{
            if (!_isInverse)
                 return Apply(point);
            else return ApplyInverted(point);
		}

        /// <summary>
        /// Transforms a list of coordinate point ordinal values.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        /// <remarks>
        /// This method is provided for efficiently transforming many points. The supplied array
        /// of ordinal values will contain packed ordinal values. For example, if the source
        /// dimension is 3, then the ordinals will be packed in this order (x0,y0,z0,x1,y1,z1 ...).
        /// The size of the passed array must be an integer multiple of DimSource. The returned
        /// ordinal values are packed in a similar way. In some DCPs. the ordinals may be
        /// transformed in-place, and the returned array may be the same as the passed array.
        /// So any client code should not attempt to reuse the passed ordinal values (although
        /// they can certainly reuse the passed array). If there is any problem then the server
        /// implementation will throw an exception. If this happens then the client should not
        /// make any assumptions about the state of the ordinal values.
        /// </remarks>
        public override List<double[]> TransformList(List<double[]> points)
		{
            List<double[]> pnts = new List<double[]>(points.Count);
            foreach (double[] p in points)
				pnts.Add(Transform(p));
			return pnts;
		}

        /// <summary>
        /// Reverses the transformation
        /// </summary>
		public override void Invert()
		{
			_isInverse = !_isInverse;
		}
	}    
}
