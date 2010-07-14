/* 
Scenic Graphics Library
Copyright (C) 2007 Jouni Tulkki

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USApackage scenic;*/
using System;
namespace scenic.filter
{
	
	/// <summary> Tent filter kernel.</summary>
	public class TentKernel:SeparableKernel
	{
		public static TentKernel Kernel
		{
			get
			{
				if (kernel == null)
					kernel = new TentKernel();
				return kernel;
			}
			
		}
		private static TentKernel kernel;
		
		private TentKernel()
		{
		}
		
		public override double getValue(double x)
		{
			if (x < - 0.5 || x > 0.5)
				return 0.0;
			return (1.0 - System.Math.Abs(x * 2.0)) * 2;
		}
	}
}