using System;
namespace Scenic.path
{
	
	public interface Path
	{
		bool Convex
		{
			get;
			
		}

		/// <summary> 
        /// Walks the given walker through the path. The error matrix is
		/// used only to ensure correct precision, but is not to used
		/// to transform the path itself.
		/// </summary>
		/// <param name="walker">the walker
		/// </param>
		/// <param name="errorMatrix">the error matrix
		/// </param>
		/// <param name="error">maximum deviation from correct path
		/// </param>
		void  walk(PathWalker walker, System.Drawing.Drawing2D.Matrix errorMatrix, double error);
		
		/// <summary> 
        /// Calculates the bounds of this path using the given transform.
		/// </summary>
		/// <param name="transform">the transform
		/// </param>
		/// <returns> the bounds
		/// </returns>
		System.Drawing.Rectangle getBounds(System.Drawing.Drawing2D.Matrix transform);
	}
}