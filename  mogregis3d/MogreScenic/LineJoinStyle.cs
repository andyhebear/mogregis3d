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
namespace Scenic
{
    /// <summary> Contains constants for different line join styles.</summary>
    public enum LineJoinStyle
    {
        /// <summary> Miter join.</summary>
        MITER_JOIN = 0,
        /// <summary> Round join.</summary>
        ROUND_JOIN = 1,
        /// <summary> Bevel join.</summary>
        BEVEL_JOIN = 2
    }
}