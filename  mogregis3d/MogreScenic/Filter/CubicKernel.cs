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
namespace Scenic.filter
{
	
	/// <summary> Cubic filter kernel.</summary>
	public class CubicKernel:SeparableKernel
	{
		private double b;
		private double c;
		
		public CubicKernel(double b, double c)
		{
			this.b = b;
			this.c = c;
		}
		
		public override double getValue(double t)
		{
			double a;
			
			t = System.Math.Abs(t * 4.0);
			
			if (t <= 1.0)
			{
				a = (12.0 - 9.0 * b - 6.0 * c) * t * t * t + (- 18.0 + 12.0 * b + 6.0 * c) * t * t + (6.0 - 2.0 * b);
			}
			else if (t <= 2.0)
			{
				a = (- b - 6.0 * c) * t * t * t + (6.0 * b + 30.0 * c) * t * t + ((- 12.0) * b - 48.0 * c) * t + (8.0 * b + 24.0 * c);
			}
			else
			{
				a = 0.0;
			}
			
			return a / 6.0 * 4.0;
		}
	}
}