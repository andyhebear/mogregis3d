using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using SharpMap.Geometries;

namespace MogreGis
{
    public class SharpMapSpatialReference : SpatialReference//, ICoordinateSystem
    {
        // Un spatialreference es para nosotros como la combinacion de Coord system y de Mathtransform
        // en la factoria se deben poder crear pasando una matrix y dejar implementado un MatrixTransform (seguir el modelo de DatumTransform)
        // o dando el cood system de destino y de origen (para que la factoria de transformadas pueda crear la mathtransform)

        ICoordinateSystem cs;

        public ICoordinateSystem CoordinateSystem
        {
            get { return cs; }
            set { cs = value; }
        }
        IMathTransform mt;

        public IMathTransform MathTransform
        {
            get { return mt; }
            set { mt = value; }
        }

        public override string getName()
        {
            return cs.Name;
        }

        public override string getWKT()
        {
            return cs.WKT;
        }

        public override bool isGeocentric()
        {
            return cs is IGeocentricCoordinateSystem;
        }

        public override bool isGeographic()
        {
            return cs is IGeographicCoordinateSystem;
        }

        public override bool isProjected()
        {
            return cs is IProjectedCoordinateSystem;
        }

        public override Ellipsoid getEllipsoid()
        {
            if (cs is IHorizontalCoordinateSystem)
            {
                Ellipsoid result;
                IHorizontalCoordinateSystem hs = (IHorizontalCoordinateSystem)cs;
                result = new Ellipsoid(hs.HorizontalDatum.Ellipsoid.SemiMajorAxis, hs.HorizontalDatum.Ellipsoid.SemiMinorAxis);
                return result;
            }
            return null;
        }

        public override SpatialReference getGeographicSRS()
        {
            if (isGeographic())
            {
                return this;
            }
            else
            {  
                ICoordinateTransformationFactory ctf = new CoordinateTransformationFactory ();
               // mt = ctf.
            }
            throw new NotImplementedException();
        }

        public override Mogre.Matrix4 getInverseReferenceFrame()
        {
            throw new NotImplementedException();
        }

        public override Mogre.Matrix4 getReferenceFrame()
        {
            throw new NotImplementedException();
        }

        public override bool equivalentTo(SpatialReference rhs)
        {
            //return this.WKT == rhs.WKT;
            throw new NotImplementedException();
        }

        public override Sharp3D.Math.Core.Vector3D getUpVector(Sharp3D.Math.Core.Vector3D at)
        {
            return base.getUpVector(at);
        }

        public override GeoExtent transform(GeoExtent input)
        {
            throw new NotImplementedException();
        }

        public override GeoPoint transform(GeoPoint input)
        {   //usar MathTransform.transform matTransform es abstracta, necesito usar la concreta de cada caso, geografica, etc.
            //la concreta me la da una factoria coordinateTransformationFactory.
            throw new NotImplementedException();
        }

        public override GeoShape transform(GeoShape input)
        {
            throw new NotImplementedException();
        }
        public  Geometry transform(Geometry input)
        {
            throw new NotImplementedException();
            //MathTransform.TransformList();
        }
        public override bool transformInPlace(GeoPoint input)
        {
            //SharpMapSpatialReference input_sr = (SharpMapSpatialReference)input.getSRS();
            //if (input_sr != null)
            //{
            //    if (input_sr.isGeocentric())
            //    {
            //        input.set(input * input_sr.getInverseReferenceFrame());
            //        Sharp3D.Math.Core.Vector3D temp = input_sr.getEllipsoid().geocentricToLatLong(new Sharp3D.Math.Core.Vector3D(input.X, input.Y, input.Z));
            //        input = new GeoPoint(temp, input_sr.getGeographicSRS());
            //        input_sr = (SharpMapSpatialReference)input.getSRS();
            //    }
            //    Sharp3D.Math.Core.Vector3D input_vec = input;
            //    bool crs_equiv = false;
            //    bool mat_equiv = false;
            //    testEquivalence(input_sr, crs_equiv, mat_equiv);

            //}

            throw new NotImplementedException();
        }
        public override bool transformInPlace(Geometry input)
        {
            throw new NotImplementedException();
            //MathTransform.TransformList();
        }

        public override bool transformInPlace(GeoShape input)
        {
            throw new NotImplementedException();
        }

        public override SpatialReference cloneWithNewReferenceFrame(Mogre.Matrix4 rf)
        {
            throw new NotImplementedException();
        }
    }
}
