using System;
using Util = scenic.Util;
namespace scenic.path
{
#if PENDING
	
	public class AWTShapePath : Path
	{
		virtual public bool Convex
		{
			get
			{
				return shape is System.Drawing.Drawing2D.GraphicsPath || shape is System.Drawing.RectangleF;
			}
			
		}
		//UPGRADE_TODO: Interface 'java.awt.Shape' was converted to 'System.Drawing.Drawing2D.GraphicsPath' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_3"'
		public System.Drawing.Drawing2D.GraphicsPath shape;
		private static double[] points = new double[10000];
		
		//UPGRADE_TODO: Interface 'java.awt.Shape' was converted to 'System.Drawing.Drawing2D.GraphicsPath' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_3"'
		public AWTShapePath(System.Drawing.Drawing2D.GraphicsPath shape)
		{
			this.shape = shape;
		}
		
		private void  walk(PathWalker walker, double[] points, int l, bool closed)
		{
			walker.beginSubPath(closed);
			for (int i = 0; i < l; i += 2)
			{
				walker.lineTo(points[i], points[i + 1]);
			}
			walker.endSubPath();
		}
		
		public virtual Path createGenericPath()
		{
			PathBuilder pathBuilder = new PathBuilder();
			//UPGRADE_TODO: Interface 'java.awt.geom.PathIterator' was converted to 'System.Drawing.Drawing2D.GraphicsPathIterator' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_3"'
			//UPGRADE_TODO: Method 'java.awt.Shape.getPathIterator' was converted to 'System.Drawing.Drawing2D.GraphicsPathIterator.GraphicsPathIterator' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtShapegetPathIterator_javaawtgeomAffineTransform_3"'
			System.Drawing.Drawing2D.GraphicsPathIterator itr = new System.Drawing.Drawing2D.GraphicsPathIterator(shape);
			double[] point = new double[6];
			
			//UPGRADE_ISSUE: Method 'java.awt.geom.PathIterator.isDone' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratorisDone_3"'
			while (!itr.isDone())
			{
				//UPGRADE_ISSUE: Method 'java.awt.geom.PathIterator.currentSegment' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratorcurrentSegment_double[]_3"'
				int type = itr.currentSegment(point);
				
				//UPGRADE_ISSUE: Method 'java.awt.geom.PathIterator.next' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratornext_3"'
				itr.next();
				//UPGRADE_ISSUE: Field 'java.awt.geom.PathIterator.SEG_MOVETO' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratorSEG_MOVETO_f_3"'
				if (type == PathIterator.SEG_MOVETO)
				{
					pathBuilder.moveTo(point[0], point[1]);
				}
				else
				{
					//UPGRADE_ISSUE: Field 'java.awt.geom.PathIterator.SEG_LINETO' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratorSEG_LINETO_f_3"'
					if (type == PathIterator.SEG_LINETO)
					{
						pathBuilder.lineTo(point[0], point[1]);
					}
					else
					{
						//UPGRADE_ISSUE: Field 'java.awt.geom.PathIterator.SEG_QUADTO' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratorSEG_QUADTO_f_3"'
						if (type == PathIterator.SEG_QUADTO)
						{
							System.Drawing.PointF p1 = new System.Drawing.PointF((float) point[0], (float) point[1]);
							System.Drawing.PointF p2 = new System.Drawing.PointF((float) point[2], (float) point[3]);
							//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
							pathBuilder.curveTo(ref p1, ref p2);
						}
						else
						{
							//UPGRADE_ISSUE: Field 'java.awt.geom.PathIterator.SEG_CUBICTO' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratorSEG_CUBICTO_f_3"'
							if (type == PathIterator.SEG_CUBICTO)
							{
								System.Drawing.PointF p1 = new System.Drawing.PointF((float) point[0], (float) point[1]);
								System.Drawing.PointF p2 = new System.Drawing.PointF((float) point[2], (float) point[3]);
								System.Drawing.PointF p3 = new System.Drawing.PointF((float) point[4], (float) point[5]);
								//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
								pathBuilder.curveTo(ref p1, ref p2, ref p3);
							}
							else
							{
								//UPGRADE_ISSUE: Field 'java.awt.geom.PathIterator.SEG_CLOSE' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratorSEG_CLOSE_f_3"'
								if (type == PathIterator.SEG_CLOSE)
								{
									pathBuilder.close();
								}
							}
						}
					}
				}
			}
			
			return pathBuilder.createPath();
		}
		
		public virtual void  walk(PathWalker walker, System.Drawing.Drawing2D.Matrix errorMatrix, double error)
		{
			//UPGRADE_TODO: Interface 'java.awt.geom.PathIterator' was converted to 'System.Drawing.Drawing2D.GraphicsPathIterator' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_3"'
			//UPGRADE_TODO: Method 'java.awt.Shape.getPathIterator' was converted to 'System.Drawing.Drawing2D.GraphicsPathIterator.GraphicsPathIterator' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtShapegetPathIterator_javaawtgeomAffineTransform_double_3"'
			System.Drawing.Drawing2D.GraphicsPathIterator itr = new System.Drawing.Drawing2D.GraphicsPathIterator(shape);
			double[] point = new double[6];
			int i = 0;
			System.Drawing.Drawing2D.Matrix inv;
			double m00, m01, m10, m11, dx, dy;
			
			try
			{
				System.Drawing.Drawing2D.Matrix temp_Matrix;
				temp_Matrix = new System.Drawing.Drawing2D.Matrix();
				temp_Matrix = errorMatrix.Clone();
				temp_Matrix.Invert();
				inv = temp_Matrix;
			}
			catch (System.Exception e)
			{
				return ;
			}
			
			m00 = (float) inv.Elements.GetValue(0);
			m01 = (float) inv.Elements.GetValue(2);
			m10 = (float) inv.Elements.GetValue(1);
			m11 = (float) inv.Elements.GetValue(3);
			dx = (System.Single) inv.OffsetX;
			dy = (System.Single) inv.OffsetY;
			
			//UPGRADE_ISSUE: Method 'java.awt.geom.PathIterator.isDone' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratorisDone_3"'
			while (!itr.isDone())
			{
				//UPGRADE_ISSUE: Method 'java.awt.geom.PathIterator.currentSegment' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratorcurrentSegment_double[]_3"'
				int type = itr.currentSegment(point);
				
				//UPGRADE_ISSUE: Method 'java.awt.geom.PathIterator.next' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratornext_3"'
				itr.next();
				//UPGRADE_ISSUE: Field 'java.awt.geom.PathIterator.SEG_MOVETO' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratorSEG_MOVETO_f_3"'
				if (type == PathIterator.SEG_MOVETO)
				{
					if (i > 0)
					{
						walk(walker, points, i, false);
						i = 0;
					}
					points[i++] = m00 * point[0] + m01 * point[1] + dx;
					points[i++] = m10 * point[0] + m11 * point[1] + dy;
				}
				else
				{
					//UPGRADE_ISSUE: Field 'java.awt.geom.PathIterator.SEG_LINETO' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratorSEG_LINETO_f_3"'
					if (type == PathIterator.SEG_LINETO)
					{
						points[i++] = m00 * point[0] + m01 * point[1] + dx;
						points[i++] = m10 * point[0] + m11 * point[1] + dy;
					}
					else
					{
						//UPGRADE_ISSUE: Field 'java.awt.geom.PathIterator.SEG_CLOSE' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomPathIteratorSEG_CLOSE_f_3"'
						if (type == PathIterator.SEG_CLOSE)
						{
							if (i > 0)
							{
								walk(walker, points, i, true);
								i = 0;
							}
						}
					}
				}
			}
			if (i > 0)
			{
				walk(walker, points, i, false);
				i = 0;
			}
		}
		
		public virtual System.Drawing.Rectangle getBounds(System.Drawing.Drawing2D.Matrix transform)
		{
			System.Drawing.Rectangle tempAux = System.Drawing.Rectangle.Truncate(shape.GetBounds());
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			return Util.transform(transform, ref tempAux);
		}
	}
#endif
}