//
// In order to convert some functionality to Visual C#, the Java Language Conversion Assistant
// creates "support classes" that duplicate the original functionality.  
//
// Support classes replicate the functionality of the original code, but in some cases they are 
// substantially different architecturally. Although every effort is made to preserve the 
// original architecture of the application in the converted project, the user should be aware that 
// the primary goal of these support classes is to replicate functionality, and that at times 
// the architecture of the resulting solution may differ somewhat.
//

using System;

	/// <summary>
	/// This interface should be implemented by any class whose instances are intended 
	/// to be executed by a thread.
	/// </summary>
	public interface IThreadRunnable
	{
		/// <summary>
		/// This method has to be implemented in order that starting of the thread causes the object's 
		/// run method to be called in that separately executing thread.
		/// </summary>
		void Run();
	}

/// <summary>
/// Contains conversion support elements such as classes, interfaces and static methods.
/// </summary>
public class SupportClass
{
	/// <summary>
	/// This class contains support methods to work with GraphicsPath and Lines.
	/// </summary>
	public class Line2DSupport
	{
		/// <summary>
		/// Creates a GraphicsPath object and adds a line to it.
		/// </summary>
		/// <param name="x1">The x-coordinate of the starting point of the line.</param>
		/// <param name="y1">The y-coordinate of the starting point of the line.</param>
		/// <param name="x2">The x-coordinate of the endpoint of the line.</param>
		/// <param name="y2">The y-coordinate of the endpoint of the line.</param>
		/// <returns>Returns a GraphicsPath object containing the line.</returns>
		public static System.Drawing.Drawing2D.GraphicsPath CreateLine2DPath(float x1, float y1, float x2, float y2)
		{
			System.Drawing.Drawing2D.GraphicsPath linePath = new System.Drawing.Drawing2D.GraphicsPath();
			linePath.AddLine(x1, y1, x2, y2);
			return linePath;
		}

		/// <summary>
		/// Creates a GraphicsPath object and adds a line to it.
		/// </summary>
		/// <param name="p1">The starting point of the line.</param>
		/// <param name="p2">The endpoint of the line.</param>
		/// <returns>Returns a GraphicsPath object containing the line</returns>
		public static System.Drawing.Drawing2D.GraphicsPath CreateLine2DPath(System.Drawing.PointF p1, System.Drawing.PointF p2)
		{
			System.Drawing.Drawing2D.GraphicsPath linePath = new System.Drawing.Drawing2D.GraphicsPath();
			linePath.AddLine(p1, p2);
			return linePath;
		}

		/// <summary>
		/// Resets the specified GraphicsPath object an adds a line to it with the specified values.
		/// </summary>
		/// <param name="linePath">The GraphicsPath object to reset.</param>
		/// <param name="x1">The x-coordinate of the starting point of the line.</param>
		/// <param name="y1">The y-coordinate of the starting point of the line.</param>
		/// <param name="x2">The x-coordinate of the endpoint of the line.</param>
		/// <param name="y2">The y-coordinate of the endpoint of the line.</param>
		public static void SetLine(System.Drawing.Drawing2D.GraphicsPath linePath, float x1, float y1, float x2, float y2)
		{
			linePath.Reset();
			linePath.AddLine(x1, y1, x2, y2);
		}

		/// <summary>
		/// Resets the specified GraphicsPath object an adds a line to it with the specified values.
		/// </summary>
		/// <param name="linePath">The GraphicsPath object to reset.</param>
		/// <param name="p1">The starting point of the line.</param>
		/// <param name="p2">The endpoint of the line.</param>
		public static void SetLine(System.Drawing.Drawing2D.GraphicsPath linePath, System.Drawing.PointF p1, System.Drawing.PointF p2)
		{
			linePath.Reset();
			linePath.AddLine(p1, p2);
		}

		/// <summary>
		/// Resets the specified GraphicsPath object an adds a line to it.
		/// </summary>
		/// <param name="linePath">The GraphicsPath object to reset.</param>
		/// <param name="newLinePath">The line to add.</param>
		public static void SetLine(System.Drawing.Drawing2D.GraphicsPath linePath, System.Drawing.Drawing2D.GraphicsPath newLinePath)
		{
			linePath.Reset();
			linePath.AddPath(newLinePath, false);
		}
	}


	/*******************************/
	/// <summary>
	/// Give functions to obtain information of graphic elements
	/// </summary>
	public class GraphicsManager
	{
		//Instance of GDI+ drawing surfaces graphics hashtable
		static public GraphicsHashTable manager = new GraphicsHashTable();

		/// <summary>
		/// Creates a new Graphics object from the device context handle associated with the Graphics
		/// parameter
		/// </summary>
		/// <param name="oldGraphics">Graphics instance to obtain the parameter from</param>
		/// <returns>A new GDI+ drawing surface</returns>
		public static System.Drawing.Graphics CreateGraphics(System.Drawing.Graphics oldGraphics)
		{
			System.Drawing.Graphics createdGraphics;
			System.IntPtr hdc = oldGraphics.GetHdc();
			createdGraphics = System.Drawing.Graphics.FromHdc(hdc);
			oldGraphics.ReleaseHdc(hdc);
			return createdGraphics;
		}

		/// <summary>
		/// This method draws a Bezier curve.
		/// </summary>
		/// <param name="graphics">It receives the Graphics instance</param>
		/// <param name="array">An array of (x,y) pairs of coordinates used to draw the curve.</param>
		public static void Bezier(System.Drawing.Graphics graphics, int[] array)
		{
			System.Drawing.Pen pen;
			pen = GraphicsManager.manager.GetPen(graphics);
			try
			{
				graphics.DrawBezier(pen, array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7]);
			}
			catch(System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException(e.ToString());
			}
		}

		/// <summary>
		/// Gets the text size width and height from a given GDI+ drawing surface and a given font
		/// </summary>
		/// <param name="graphics">Drawing surface to use</param>
		/// <param name="graphicsFont">Font type to measure</param>
		/// <param name="text">String of text to measure</param>
		/// <returns>A point structure with both size dimentions; x for width and y for height</returns>
		public static System.Drawing.Point GetTextSize(System.Drawing.Graphics graphics, System.Drawing.Font graphicsFont, System.String text)
		{
			System.Drawing.Point textSize;
			System.Drawing.SizeF tempSizeF;
			tempSizeF = graphics.MeasureString(text, graphicsFont);
			textSize = new System.Drawing.Point();
			textSize.X = (int) tempSizeF.Width;
			textSize.Y = (int) tempSizeF.Height;
			return textSize;
		}

		/// <summary>
		/// Gets the text size width and height from a given GDI+ drawing surface and a given font
		/// </summary>
		/// <param name="graphics">Drawing surface to use</param>
		/// <param name="graphicsFont">Font type to measure</param>
		/// <param name="text">String of text to measure</param>
		/// <param name="width">Maximum width of the string</param>
		/// <param name="format">StringFormat object that represents formatting information, such as line spacing, for the string</param>
		/// <returns>A point structure with both size dimentions; x for width and y for height</returns>
		public static System.Drawing.Point GetTextSize(System.Drawing.Graphics graphics, System.Drawing.Font graphicsFont, System.String text, System.Int32 width, System.Drawing.StringFormat format)
		{
			System.Drawing.Point textSize;
			System.Drawing.SizeF tempSizeF;
			tempSizeF = graphics.MeasureString(text, graphicsFont, width, format);
			textSize = new System.Drawing.Point();
			textSize.X = (int) tempSizeF.Width;
			textSize.Y = (int) tempSizeF.Height;
			return textSize;
		}

		/// <summary>
		/// Gives functionality over a hashtable of GDI+ drawing surfaces
		/// </summary>
		public class GraphicsHashTable:System.Collections.Hashtable 
		{
			/// <summary>
			/// Gets the graphics object from the given control
			/// </summary>
			/// <param name="control">Control to obtain the graphics from</param>
			/// <returns>A graphics object with the control's characteristics</returns>
			public System.Drawing.Graphics GetGraphics(System.Windows.Forms.Control control)
			{
				System.Drawing.Graphics graphic;
				if (control.Visible == true)
				{
					graphic = control.CreateGraphics();
				}
				else
				{
					graphic = null;
				}
				return graphic;
			}

			/// <summary>
			/// Sets the background color property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given background color.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="color">Background color to set</param>
			public void SetBackColor(System.Drawing.Graphics graphic, System.Drawing.Color color)
			{
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).BackColor = color;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.BackColor = color;
					Add(graphic, tempProps);
				}
			}

			/// <summary>
			/// Gets the background color property to the given graphics object in the hashtable. If the element doesn't exist, then it returns White.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The background color of the graphic</returns>
			public System.Drawing.Color GetBackColor(System.Drawing.Graphics graphic)
			{
				if (this[graphic] == null)
					return System.Drawing.Color.White;
				else
					return ((GraphicsProperties) this[graphic]).BackColor;
			}

			/// <summary>
			/// Sets the text color property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given text color.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="color">Text color to set</param>
			public void SetTextColor(System.Drawing.Graphics graphic, System.Drawing.Color color)
			{
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).TextColor = color;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.TextColor = color;
					Add(graphic, tempProps);
				}
			}

			/// <summary>
			/// Gets the text color property to the given graphics object in the hashtable. If the element doesn't exist, then it returns White.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The text color of the graphic</returns>
			public System.Drawing.Color GetTextColor(System.Drawing.Graphics graphic) 
			{
				if (this[graphic] == null)
					return System.Drawing.Color.White;
				else
					return ((GraphicsProperties) this[graphic]).TextColor;
			}

			/// <summary>
			/// Sets the GraphicBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given GraphicBrush.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="brush">GraphicBrush to set</param>
			public void SetBrush(System.Drawing.Graphics graphic, System.Drawing.SolidBrush brush) 
			{
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).GraphicBrush = brush;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.GraphicBrush = brush;
					Add(graphic, tempProps);
				}
			}
			
			/// <summary>
			/// Sets the GraphicBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given GraphicBrush.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="brush">GraphicBrush to set</param>
			public void SetPaint(System.Drawing.Graphics graphic, System.Drawing.Brush brush) 
			{
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).PaintBrush = brush;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.PaintBrush = brush;
					Add(graphic, tempProps);
				}
			}
			
			/// <summary>
			/// Sets the GraphicBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given GraphicBrush.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="color">Color to set</param>
			public void SetPaint(System.Drawing.Graphics graphic, System.Drawing.Color color) 
			{
				System.Drawing.Brush brush = new System.Drawing.SolidBrush(color);
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).PaintBrush = brush;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.PaintBrush = brush;
					Add(graphic, tempProps);
				}
			}


			/// <summary>
			/// Gets the HatchBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Blank.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The HatchBrush setting of the graphic</returns>
			public System.Drawing.Drawing2D.HatchBrush GetBrush(System.Drawing.Graphics graphic)
			{
				if (this[graphic] == null)
					return new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Plaid,System.Drawing.Color.Black,System.Drawing.Color.Black);
				else
					return new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Plaid,((GraphicsProperties) this[graphic]).GraphicBrush.Color,((GraphicsProperties) this[graphic]).GraphicBrush.Color);
			}
			
			/// <summary>
			/// Gets the HatchBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Blank.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The Brush setting of the graphic</returns>
			public System.Drawing.Brush GetPaint(System.Drawing.Graphics graphic)
			{
				if (this[graphic] == null)
					return new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Plaid,System.Drawing.Color.Black,System.Drawing.Color.Black);
				else
					return ((GraphicsProperties) this[graphic]).PaintBrush;
			}

			/// <summary>
			/// Sets the GraphicPen property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given Pen.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="pen">Pen to set</param>
			public void SetPen(System.Drawing.Graphics graphic, System.Drawing.Pen pen) 
			{
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).GraphicPen = pen;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.GraphicPen = pen;
					Add(graphic, tempProps);
				}
			}

			/// <summary>
			/// Gets the GraphicPen property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Black.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The GraphicPen setting of the graphic</returns>
			public System.Drawing.Pen GetPen(System.Drawing.Graphics graphic)
			{
				if (this[graphic] == null)
					return System.Drawing.Pens.Black;
				else
					return ((GraphicsProperties) this[graphic]).GraphicPen;
			}

			/// <summary>
			/// Sets the GraphicFont property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given Font.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="Font">Font to set</param>
			public void SetFont(System.Drawing.Graphics graphic, System.Drawing.Font font) 
			{
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).GraphicFont = font;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.GraphicFont = font;
					Add(graphic,tempProps);
				}
			}

			/// <summary>
			/// Gets the GraphicFont property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Microsoft Sans Serif with size 8.25.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The GraphicFont setting of the graphic</returns>
			public System.Drawing.Font GetFont(System.Drawing.Graphics graphic)
			{
				if (this[graphic] == null)
					return new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				else
					return ((GraphicsProperties) this[graphic]).GraphicFont;
			}

			/// <summary>
			/// Sets the color properties for a given Graphics object. If the element doesn't exist, then it adds the graphic element to the hashtable with the color properties set with the given value.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="color">Color value to set</param>
			public void SetColor(System.Drawing.Graphics graphic, System.Drawing.Color color) 
			{
				if (this[graphic] != null)
				{
					((GraphicsProperties) this[graphic]).GraphicPen.Color = color;
					((GraphicsProperties) this[graphic]).GraphicBrush.Color = color;
					((GraphicsProperties) this[graphic]).color = color;
				}
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.GraphicPen.Color = color;
					tempProps.GraphicBrush.Color = color;
					tempProps.color = color;
					Add(graphic,tempProps);
				}
			}

			/// <summary>
			/// Gets the color property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Black.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The color setting of the graphic</returns>
			public System.Drawing.Color GetColor(System.Drawing.Graphics graphic) 
			{
				if (this[graphic] == null)
					return System.Drawing.Color.Black;
				else
					return ((GraphicsProperties) this[graphic]).color;
			}

			/// <summary>
			/// This method gets the TextBackgroundColor of a Graphics instance
			/// </summary>
			/// <param name="graphic">The graphics instance</param>
			/// <returns>The color value in ARGB encoding</returns>
			public System.Drawing.Color GetTextBackgroundColor(System.Drawing.Graphics graphic)
			{
				if (this[graphic] == null)
					return System.Drawing.Color.Black;
				else 
				{ 
					return ((GraphicsProperties) this[graphic]).TextBackgroundColor;
				}
			}

			/// <summary>
			/// This method set the TextBackgroundColor of a Graphics instace
			/// </summary>
			/// <param name="graphic">The graphics instace</param>
			/// <param name="color">The System.Color to set the TextBackgroundColor</param>
			public void SetTextBackgroundColor(System.Drawing.Graphics graphic, System.Drawing.Color color) 
			{
				if (this[graphic] != null)
				{
					((GraphicsProperties) this[graphic]).TextBackgroundColor = color;								
				}
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.TextBackgroundColor = color;				
					Add(graphic,tempProps);
				}
			}

			/// <summary>
			/// Structure to store properties from System.Drawing.Graphics objects
			/// </summary>
			class GraphicsProperties
			{
				public System.Drawing.Color TextBackgroundColor = System.Drawing.Color.Black;
				public System.Drawing.Color color = System.Drawing.Color.Black;
				public System.Drawing.Color BackColor = System.Drawing.Color.White;
				public System.Drawing.Color TextColor = System.Drawing.Color.Black;
				public System.Drawing.SolidBrush GraphicBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
				public System.Drawing.Brush PaintBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
				public System.Drawing.Pen   GraphicPen = new System.Drawing.Pen(System.Drawing.Color.Black);
				public System.Drawing.Font  GraphicFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			}
		}
	}

	/*******************************/
	/// <summary>
	/// This class contains support methods to work with GraphicsPath and Ellipses.
	/// </summary>
	public class Ellipse2DSupport
	{
		/// <summary>
		/// Creates a object and adds an ellipse to it.
		/// </summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding Rectangle that defines the ellipse.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding Rectangle that defines the ellipse.</param>
		/// <param name="width">The width of the bounding Rectangle that defines the ellipse.</param>
		/// <param name="height">The height of the bounding Rectangle that defines the ellipse.</param>
		/// <returns>Returns a GraphicsPath object containing an ellipse.</returns>
		public static System.Drawing.Drawing2D.GraphicsPath CreateEllipsePath(float x, float y, float width, float height)
		{
			System.Drawing.Drawing2D.GraphicsPath ellipsePath = new System.Drawing.Drawing2D.GraphicsPath();
			ellipsePath.AddEllipse(x, y, width, height);
			return ellipsePath;
		}

		/// <summary>
		/// Resets the x-coordinate of the ellipse path contained in the specified GraphicsPath object.
		/// </summary>
		/// <param name="ellipsePath">The GraphicsPath object that will be set.</param>
		/// <param name="x">The new x-coordinate.</param>
		public static void SetX(System.Drawing.Drawing2D.GraphicsPath ellipsePath, float x)
		{
			System.Drawing.RectangleF rectangle = ellipsePath.GetBounds();
			rectangle.X = x;
			ellipsePath.Reset();
			ellipsePath.AddEllipse(rectangle);
		}

		/// <summary>
		/// Resets the y-coordinate of the ellipse path contained in the specified GraphicsPath object.
		/// </summary>
		/// <param name="ellipsePath">The GraphicsPath object that will be set.</param>
		/// <param name="y">The new y-coordinate.</param>
		public static void SetY(System.Drawing.Drawing2D.GraphicsPath ellipsePath, float y)
		{
			System.Drawing.RectangleF rectangle = ellipsePath.GetBounds();
			rectangle.Y = y;
			ellipsePath.Reset();
			ellipsePath.AddEllipse(rectangle);
		}

		/// <summary>
		/// Resets the width of the ellipse path contained in the specified GraphicsPath object.
		/// </summary>
		/// <param name="ellipsePath">The GraphicsPath object that will be set.</param>
		/// <param name="width">The new width.</param>
		public static void SetWidth(System.Drawing.Drawing2D.GraphicsPath ellipsePath, float width)
		{
			System.Drawing.RectangleF rectangle = ellipsePath.GetBounds();
			rectangle.Width = width;
			ellipsePath.Reset();
			ellipsePath.AddEllipse(rectangle);
		}

		/// <summary>
		/// Resets the height of the ellipse path contained in the specified GraphicsPath object.
		/// </summary>
		/// <param name="ellipsePath">The GraphicsPath object that will be set.</param>
		/// <param name="height">The new height.</param>
		public static void SetHeight(System.Drawing.Drawing2D.GraphicsPath ellipsePath, float height)
		{
			System.Drawing.RectangleF rectangle = ellipsePath.GetBounds();
			rectangle.Height = height;
			ellipsePath.Reset();
			ellipsePath.AddEllipse(rectangle);
		}
	}


	/*******************************/
	/// <summary>
	/// This class contains support methods to work with GraphicsPath and Arcs.
	/// </summary>
	public class Arc2DSupport
	{
		/// <summary>
		/// Specifies an OPEN Arc type.
		/// </summary>
		public const int OPEN = 0;
		/// <summary>
		/// Specifies an CLOSED Arc type.
		/// </summary>
		public const int CLOSED = 1;
		/// <summary>
		/// Specifies an PIE Arc type.
		/// </summary>
		public const int PIE = 2;
		/// <summary>
		/// Creates a GraphicsPath object and adds an Arc to it with the specified Arc values and closure type.
		/// </summary>
		/// <param name="x">The x coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the Arc is drawn.</param>
		/// <param name="y">The y coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the Arc is drawn.</param>
		/// <param name="height">The height of the rectangular region that defines the ellipse from which the Arc is drawn.</param>
		/// <param name="width">The width of the rectangular region that defines the ellipse from which the Arc is drawn.</param>
		/// <param name="start">The starting angle of the Arc measured in degrees.</param>
		/// <param name="extent">The angular extent of the Arc measured in degrees.</param>
		/// <param name="arcType">The closure type for the Arc.</param>
		/// <returns>Returns a new GraphicsPath object that contains the Arc path.</returns>
		public static System.Drawing.Drawing2D.GraphicsPath CreateArc2D(float x, float y, float height, float width, float start, float extent, int arcType)
		{
			System.Drawing.Drawing2D.GraphicsPath arc2DPath = new System.Drawing.Drawing2D.GraphicsPath();
			switch (arcType)
			{
				case OPEN:
					arc2DPath.AddArc(x, y, height, width, start * -1, extent * -1);
					break;
				case CLOSED:
					arc2DPath.AddArc(x, y, height, width, start * -1, extent * -1);
					arc2DPath.CloseFigure();
					break;
				case PIE:
					arc2DPath.AddPie(x, y, height, width, start * -1, extent * -1);
					break;
				default:
					break;
			}
			return arc2DPath;
		}
		/// <summary>
		/// Creates a GraphicsPath object and adds an Arc to it with the specified Arc values and closure type.
		/// </summary>
		/// <param name="ellipseBounds">A RectangleF structure that represents the rectangular bounds of the ellipse from which the Arc is taken.</param>
		/// <param name="start">The starting angle of the Arc measured in degrees.</param>
		/// <param name="extent">The angular extent of the Arc measured in degrees.</param>
		/// <param name="arcType">The closure type for the Arc.</param>
		/// <returns>Returns a new GraphicsPath object that contains the Arc path.</returns>
		public static System.Drawing.Drawing2D.GraphicsPath CreateArc2D(System.Drawing.RectangleF ellipseBounds, float start, float extent, int arcType)
		{
			System.Drawing.Drawing2D.GraphicsPath arc2DPath = new System.Drawing.Drawing2D.GraphicsPath();
			switch (arcType)
			{
				case OPEN:
					arc2DPath.AddArc(ellipseBounds, start * -1, extent * -1);
					break;
				case CLOSED:
					arc2DPath.AddArc(ellipseBounds, start * -1, extent * -1);
					arc2DPath.CloseFigure();
					break;
				case PIE:
					arc2DPath.AddPie(ellipseBounds.X, ellipseBounds.Y, ellipseBounds.Width, ellipseBounds.Height, start * -1, extent * -1);
					break;
				default:
					break;
			}

			return arc2DPath;
		}

		/// <summary>
		/// Resets the specified GraphicsPath object and adds an Arc to it with the speficied values.
		/// </summary>
		/// <param name="arc2DPath">The GraphicsPath object to reset.</param>
		/// <param name="x">The x coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the Arc is drawn.</param>
		/// <param name="y">The y coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the Arc is drawn.</param>
		/// <param name="height">The height of the rectangular region that defines the ellipse from which the Arc is drawn.</param>
		/// <param name="width">The width of the rectangular region that defines the ellipse from which the Arc is drawn.</param>
		/// <param name="start">The starting angle of the Arc measured in degrees.</param>
		/// <param name="extent">The angular extent of the Arc measured in degrees.</param>
		/// <param name="arcType">The closure type for the Arc.</param>
		public static void SetArc(System.Drawing.Drawing2D.GraphicsPath arc2DPath, float x, float y, float height, float width, float start, float extent, int arcType)
		{
			arc2DPath.Reset();
			switch (arcType)
			{
				case OPEN:
					arc2DPath.AddArc(x, y, height, width, start * -1, extent * -1);
					break;
				case CLOSED:
					arc2DPath.AddArc(x, y, height, width, start * -1, extent * -1);
					arc2DPath.CloseFigure();
					break;
				case PIE:
					arc2DPath.AddPie(x, y, height, width, start * -1, extent * -1);
					break;
				default:
					break;
			}
		}
	}
	/*******************************/
	/// <summary>
	/// Creates a GraphicsPath from two Int Arrays with a specific number of points.
	/// </summary>
	/// <param name="xPoints">Int Array to set the X points of the GraphicsPath</param>
	/// <param name="yPoints">Int Array to set the Y points of the GraphicsPath</param>
	/// <param name="pointsNumber">Number of points to add to the GraphicsPath</param>
	/// <returns>A new GraphicsPath</returns>
	public static System.Drawing.Drawing2D.GraphicsPath CreateGraphicsPath(int[] xPoints, int[] yPoints, int pointsNumber)
	{
		System.Drawing.Drawing2D.GraphicsPath tempGraphicsPath = new System.Drawing.Drawing2D.GraphicsPath();
		if (pointsNumber == 2)
			tempGraphicsPath.AddLine(xPoints[0], yPoints[0], xPoints[1], yPoints[1]);
		else
		{
			System.Drawing.Point[] tempPointArray = new System.Drawing.Point[pointsNumber];
			for (int index = 0; index < pointsNumber; index++)
				tempPointArray[index] = new System.Drawing.Point(xPoints[index], yPoints[index]);

			tempGraphicsPath.AddPolygon(tempPointArray);
		}
		return tempGraphicsPath;
	}

	/*******************************/
	/// <summary>
	/// Creates a System.Drawing.Rectangle with the giving parameters
	/// </summary>
	/// <param name="x1">The x coordinate</param>
	/// <param name="y1">The y coordinate</param>
	/// <param name="x2">The x final coordinate</param>
	/// <param name="y2">The y final coordinate</param>
	/// <returns>The new Rectangle</returns>
	public static System.Drawing.Rectangle CreateRectangle(int x1, int y1, int x2, int y2)
	{
		System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x1, y1, x2-x1, y2-y1);
		return rectangle;
	}

	/*******************************/
	/// <summary>
	/// Converts an angle in degrees to radians.
	/// </summary>
	/// <param name="angleInDegrees">Double value of angle in degrees to convert.</param>
	/// <returns>The value of the angle in radians.</returns>
	public static double DegreesToRadians(double angleInDegrees)
	{
		double valueRadians =  (2 * System.Math.PI) / 360;
		return angleInDegrees * valueRadians;
	}

	/*******************************/
/// <summary>
/// Provides Color functionality for ColorModel
/// </summary>
public class ColorSupport
{
	/// <summary>
	/// Gets the Alpha color from the giving color
	/// </summary>
	/// <param name="color">The color to use</param>
	/// <returns>The Alpha color</returns>
	public static int GetAlphaFromColor(int color)
	{
		System.Drawing.Color newColor = System.Drawing.Color.FromArgb(color);
		return newColor.A;
	}

	/// <summary>
	/// Gets the Red color from the giving color
	/// </summary>
	/// <param name="color">The color to use</param>
	/// <returns>The Red color</returns>
	public static int GetRedFromColor(int color)
	{
		System.Drawing.Color newColor = System.Drawing.Color.FromArgb(color);
		return newColor.R;
	}

	/// <summary>
	/// Gets the Green color from the giving color
	/// </summary>
	/// <param name="color">The color to use</param>
	/// <returns>The Green color</returns>
	public static int GetGreenFromColor(int color)
	{
		System.Drawing.Color newColor = System.Drawing.Color.FromArgb(color);
		return newColor.G;
	}

	/// <summary>
	/// Gets the Blue color from the giving color
	/// </summary>
	/// <param name="color">The color to use</param>
	/// <returns>The Blue color</returns>
	public static int GetBlueFromColor(int color)
	{
		System.Drawing.Color newColor = System.Drawing.Color.FromArgb(color);
		return newColor.B;
	}

	/// <summary>
	/// Gets the RGB color from the giving color
	/// </summary>
	/// <param name="color">The color to use</param>
	/// <returns>The RGB value color</returns>
	public static int GetRGBFromColor(int color)
	{
		System.Drawing.Color newColor = System.Drawing.Color.FromArgb(color);
		return newColor.ToArgb();
	}

	/// <summary>
	/// Returns the default bitsize of the pixel
	/// </summary>
	/// <returns>The default for .NET is 32</returns>
	public static int GetColorPixelSize()
	{
		return 32;
	}

	/// <summary>
	/// Returns the default bitsize of the pixel
	/// </summary>
	public static int PixelBits
	{
		get
		{
			return 32;
		}
	}

	/// <summary>
	/// Creates a new intance of System.Drawing.Color
	/// </summary>
	/// <returns>A new instance with de default values for System.Drawing.Color</returns>
	public static System.Drawing.Color GetRGBDefault()
	{
		return new System.Drawing.Color();
	}

	/// <summary>
	/// Creates the mask for Alpha color
	/// </summary>
	/// <returns>The int value of the mask for alpha color</returns>
	public static uint GetAlphaMask()
	{
		return (0xFF000000);
	}

	/// <summary>
	/// Creates the mask for Red color
	/// </summary>
	/// <returns>The int value of the mask for red color</returns>
	public static int GetRedMask()
	{
		return (0x00FF0000);
	}

	/// <summary>
	/// Creates the mask for Green color
	/// </summary>
	/// <returns>The int value of the mask for green color</returns>
	public static int GetGreenMask()
	{
		return (0x0000FF00);
	}

	/// <summary>
	/// Creates the mask for Blue color
	/// </summary>
	/// <returns>The int value of the mask for blue color</returns>
	public static int GetBlueMask()
	{
		return (0x000000FF);
	}
}
	/*******************************/
	/// <summary>
	/// This class is used for storing colors in an array.  It can be used for constructing custom color palettes.
	/// </summary>
	public class IndexedColorArray
	{
		/// <summary>
		/// The array of color values.
		/// </summary>
		private int[] colorArray;
			
		/// <summary>
		/// The size of the array of color values.
		/// </summary>
		private int arraySize;
			
		/// <summary>
		/// Bitsize of color values.  Not Used.  Provided for compatibility.
		/// </summary>
		private int bitSize;

		/// <summary>
		/// Position in the array of the transparency value.
		/// </summary>
		private int transparentPixel;

		/// <summary>
		/// Size Property.
		/// <returns>The size of the array of color components.</returns>
		/// </summary>
		public int Size
		{
			get
			{
				return this.arraySize;
			}
		}

		/// <summary>
		/// TransparentPixel Property.
		/// </summary>
		/// <returns>The position of the transparency pixel within the array.</returns>
		public int TransparentPixel
		{ 
			get
			{
				return this.transparentPixel;
			}
		}

		/// <summary>
		/// Creates an indexed color array with arrays of color components.
		/// </summary>
		/// <param name="bitSize">BitSize of pixel values. (Not used. Supplied for compatibility.)</param>
		/// <param name="size">Size of the IndexedColorArray.</param>
		/// <param name="red">Array of red color components.</param>
		/// <param name="green">Array of green color components.</param>
		/// <param name="blue">Array of blue color components.</param>
		public IndexedColorArray(int bitSize, int size, byte[] red, byte[] green, byte[] blue)
		{
			this.bitSize = bitSize;
			SetValues(size, red, green, blue, null);
		}

		/// <summary>
		/// Creates an indexed color array with arrays of color components.
		/// </summary>
		/// <param name="bitSize">BitSize of pixel values. (Not used. Supplied for compatibility.)</param>
		/// <param name="size">Size of the IndexedColorArray.</param>
		/// <param name="red">Array of red color components.</param>
		/// <param name="green">Array of green color components.</param>
		/// <param name="blue">Array of blue color components.</param>
		/// <param name="transparencyPixel">Position in the array of the transparency pixel.</param>
		public IndexedColorArray(int bitSize, int size, byte[] red, byte[] green, byte[] blue, int transparencyPixel)
		{
			this.bitSize = bitSize;
			SetValues(size, red, green, blue, null);
			this.transparentPixel = transparencyPixel;
		}

		/// <summary>
		/// Creates an indexed color array with arrays of color components.
		/// </summary>
		/// <param name="bitSize">BitSize of pixel values. (Not used. Supplied for compatibility.)</param>
		/// <param name="size">Size of the IndexedColorArray.</param>
		/// <param name="red">Array of red color components.</param>
		/// <param name="green">Array of green color components.</param>
		/// <param name="blue">Array of blue color components.</param>
		/// <param name="alpha">Array of alpha values.</param>
		public IndexedColorArray(int bitSize, int size, byte[] red, byte[] green, byte[] blue, byte[] alpha)
		{
			this.bitSize = bitSize;
			SetValues(size, red, green, blue, alpha);
		}

		/// <summary>
		/// Creates an indexed color array with an array of color components.
		/// </summary>
		/// <param name="bitSize">BitSize of pixel values. (Not used. Supplied for compatibility.)</param>
		/// <param name="size">Size of the IndexedColorArray</param>
		/// <param name="colorMap">Array of color components</param>
		/// <param name="startPosition">Position in the array to start reading values.</param>
		/// <param name="hasAlpha">Boolean value indicating the presence of alpha values in the color components.</param>
		public IndexedColorArray(int bitSize, int size, byte[] colorMap, int startPosition,	bool hasAlpha) : this(bitSize, size, colorMap, startPosition, hasAlpha, -1)
		{
		}

		/// <summary>
		/// Creates an indexed color array with an array of color components.
		/// </summary>
		/// <param name="bitSize">BitSize of pixel values. (Not used. Supplied for compatibility.)</param>
		/// <param name="size">Size of the IndexedColorArray.</param>
		/// <param name="colorMap">Array of color components.</param>
		/// <param name="startPosition">Position in the array to start reading values.</param>
		/// <param name="hasAlpha">Boolean value indicating the presence of alpha values in the color components.</param>
		/// <param name=" transparencyPixel">Position within the array of the transparency pixel.</param>
		public IndexedColorArray(int bitSize, int size, byte[] colorMap, int startPosition, bool hasAlpha, int transparencyPixel) 
		{
			this.bitSize = bitSize;
			this.arraySize = size;
			int maxsize = size > 256 ? size : 256;
			this.colorArray = new int[maxsize];
			int start = startPosition;
			int alphaMask = 0xff;
			for (int index = 0; index < size; index++) 
			{
				this.colorArray[index] = ((colorMap[start++] & 0xff) << 16) | ((colorMap[start++] & 0xff) << 8) | (colorMap[start++] & 0xff);
				this.colorArray[index] |= (alphaMask << 24);
			}
			this.transparentPixel = transparencyPixel;
		}

		/// <summary>
		/// Stores color components. It converts byte array parameters to color components.</para>
		/// </summary>
		/// <param name="size">Size of the array of color components.</param>
		/// <param name="red">Array of red color components.</param>
		/// <param name="green">Array of green color components.</param>
		/// <param name="blue">Array of blue color components.</param>
		/// <param name="alpha">Array of alpha values.</param>
		/// <remarks> This method is private.</remarks>
		private void SetValues(int size, byte[] red, byte[] green, byte[] blue, byte[] alpha)
		{
			this.arraySize = size;
			int maxsize = this.arraySize > 256 ? this.arraySize : 256;
			this.colorArray = new int[maxsize];
			int alphaMask = 0xff;
			for (int index = 0; index < size; index++) 
				this.colorArray[index] = (alphaMask << 24) | ((red[index] & 0xff) << 16) | ((green[index] & 0xff) << 8) | (blue[index] & 0xff);
		}

		/// <summary>
		/// Returns the array of red color components in the byte array passed as parameter.
		/// </summary>
		/// <param name="red">The array to store the red color components.</param>
		/// <returns>The values are returned in the array passed as parameter.</returns>
		public void GetRedValues(byte[] red)
		{
			for (int index = 0; index < this.arraySize; index++) 
				red[index] = (byte) (this.colorArray[index] >> 16);
		}

		/// <summary>
		/// Returns the array of green color components in the byte array passed as parameter.
		/// </summary>
		/// <param name="green">The array to store the green color components.</param>
		/// <returns>The values are returned in the array passed as parameter.</returns>
		public void GetGreenValues(byte[] green)
		{
			for (int index = 0; index < this.arraySize; index++)
				green[index] = (byte) (this.colorArray[index] >> 8);
		}

		/// <summary>
		/// Returns the array of blue color components in the byte array passed as parameter.
		/// </summary>
		/// <param name="blue">The array to store the blue color components.</param>
		/// <returns>The values are returned in the array passed as parameter.</returns>
		public void GetBlueValues(byte[] blue)
		{
			for (int index = 0; index < this.arraySize; index++)
				blue[index] = (byte) this.colorArray[index];
		}

		/// <summary>
		/// Returns the array of alpha color components in the byte array passed as parameter.
		/// </summary>
		/// <param name="alpha">The array to store the blue color components.</param>
		/// <returns>The values are returned in the array passed as parameter.</returns>
		public void GetAlphaValues(byte[] alpha) 
		{
			for (int index = 0; index < this.arraySize; index++)
				alpha[index] = (byte) (this.colorArray[index] >> 24);
		}

		/// <summary>
		/// Returns the red color component of the pixel at location passed as parameter.
		/// </summary>
		/// <param name="pixel">Location of pixel to get the red color.</param>
		/// <returns>The red color component.</returns>
		public int GetRedFromPixel(int pixel) 
		{
			return (this.colorArray[pixel] >> 16) & 0xff;
		}

		/// <summary>
		/// Returns the green color component of the pixel at location passed as parameter.
		/// </summary>
		/// <param name="pixel">Location of pixel to get the red color.</param>
		/// <returns>The green color component.</returns>
		public int GetGreenFromPixel(int pixel)
		{
			return (this.colorArray[pixel] >> 8) & 0xff;
		}

		/// <summary>
		/// Returns the blue color component of the pixel at location passed as parameter.
		/// </summary>
		/// <param name="pixel">Location of pixel to get the blue color.</param>
		/// <returns>The blue color component.</returns>
		public int GetBlueFromPixel(int pixel)
		{
			return this.colorArray[pixel] & 0xff;
		}

		/// <summary>
		/// Returns the alpha color component of the pixel at location passed as parameter.
		/// </summary>
		/// <param name="pixel">Location of pixel to get the alpha componenet.</param>
		/// <returns>The alpha color component.</returns>
		public int GetAlphaFromPixel(int pixel)
		{
			return (this.colorArray[pixel] >> 24) & 0xff;
		}

		/// <summary>
		/// GetARGBFromPixel Method.
		/// </summary>
		/// <param name="pixel">Location of pixel to get color value.</param>
		/// <returns>The value of the color.</returns>
		public int GetARGBFromPixel(int pixel)
		{
			return this.colorArray[pixel];
		}
	}

	/*******************************/
	/// <summary>
	/// Receives a byte array and returns it transformed in an sbyte array
	/// </summary>
	/// <param name="byteArray">Byte array to process</param>
	/// <returns>The transformed array</returns>
	public static sbyte[] ToSByteArray(byte[] byteArray)
	{
		sbyte[] sbyteArray = null;
		if (byteArray != null)
		{
			sbyteArray = new sbyte[byteArray.Length];
			for(int index=0; index < byteArray.Length; index++)
				sbyteArray[index] = (sbyte) byteArray[index];
		}
		return sbyteArray;
	}

	/*******************************/
	/// <summary>
	/// Support class used to handle threads
	/// </summary>
	public class ThreadClass : IThreadRunnable
	{
		/// <summary>
		/// The instance of System.Threading.Thread
		/// </summary>
		private System.Threading.Thread threadField;
	      
		/// <summary>
		/// Initializes a new instance of the ThreadClass class
		/// </summary>
		public ThreadClass()
		{
			threadField = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
		}
	 
		/// <summary>
		/// Initializes a new instance of the Thread class.
		/// </summary>
		/// <param name="Name">The name of the thread</param>
		public ThreadClass(System.String Name)
		{
			threadField = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
			this.Name = Name;
		}
	      
		/// <summary>
		/// Initializes a new instance of the Thread class.
		/// </summary>
		/// <param name="Start">A ThreadStart delegate that references the methods to be invoked when this thread begins executing</param>
		public ThreadClass(System.Threading.ThreadStart Start)
		{
			threadField = new System.Threading.Thread(Start);
		}
	 
		/// <summary>
		/// Initializes a new instance of the Thread class.
		/// </summary>
		/// <param name="Start">A ThreadStart delegate that references the methods to be invoked when this thread begins executing</param>
		/// <param name="Name">The name of the thread</param>
		public ThreadClass(System.Threading.ThreadStart Start, System.String Name)
		{
			threadField = new System.Threading.Thread(Start);
			this.Name = Name;
		}
	      
		/// <summary>
		/// This method has no functionality unless the method is overridden
		/// </summary>
		public virtual void Run()
		{
		}
	      
		/// <summary>
		/// Causes the operating system to change the state of the current thread instance to ThreadState.Running
		/// </summary>
		public virtual void Start()
		{
			threadField.Start();
		}
	      
		/// <summary>
		/// Interrupts a thread that is in the WaitSleepJoin thread state
		/// </summary>
		public virtual void Interrupt()
		{
			threadField.Interrupt();
		}
	      
		/// <summary>
		/// Gets the current thread instance
		/// </summary>
		public System.Threading.Thread Instance
		{
			get
			{
				return threadField;
			}
			set
			{
				threadField = value;
			}
		}
	      
		/// <summary>
		/// Gets or sets the name of the thread
		/// </summary>
		public System.String Name
		{
			get
			{
				return threadField.Name;
			}
			set
			{
				if (threadField.Name == null)
					threadField.Name = value; 
			}
		}
	      
		/// <summary>
		/// Gets or sets a value indicating the scheduling priority of a thread
		/// </summary>
		public System.Threading.ThreadPriority Priority
		{
			get
			{
				return threadField.Priority;
			}
			set
			{
				threadField.Priority = value;
			}
		}
	      
		/// <summary>
		/// Gets a value indicating the execution status of the current thread
		/// </summary>
		public bool IsAlive
		{
			get
			{
				return threadField.IsAlive;
			}
		}
	      
		/// <summary>
		/// Gets or sets a value indicating whether or not a thread is a background thread.
		/// </summary>
		public bool IsBackground
		{
			get
			{
				return threadField.IsBackground;
			} 
			set
			{
				threadField.IsBackground = value;
			}
		}
	      
		/// <summary>
		/// Blocks the calling thread until a thread terminates
		/// </summary>
		public void Join()
		{
			threadField.Join();
		}
	      
		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses
		/// </summary>
		/// <param name="MiliSeconds">Time of wait in milliseconds</param>
		public void Join(long MiliSeconds)
		{
			lock(this)
			{
				threadField.Join(new System.TimeSpan(MiliSeconds * 10000));
			}
		}
	      
		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses
		/// </summary>
		/// <param name="MiliSeconds">Time of wait in milliseconds</param>
		/// <param name="NanoSeconds">Time of wait in nanoseconds</param>
		public void Join(long MiliSeconds, int NanoSeconds)
		{
			lock(this)
			{
				threadField.Join(new System.TimeSpan(MiliSeconds * 10000 + NanoSeconds * 100));
			}
		}
	      
		/// <summary>
		/// Resumes a thread that has been suspended
		/// </summary>
		public void Resume()
		{
			threadField.Resume();
		}
	      
		/// <summary>
		/// Raises a ThreadAbortException in the thread on which it is invoked, 
		/// to begin the process of terminating the thread. Calling this method 
		/// usually terminates the thread
		/// </summary>
		public void Abort()
		{
			threadField.Abort();
		}
	      
		/// <summary>
		/// Raises a ThreadAbortException in the thread on which it is invoked, 
		/// to begin the process of terminating the thread while also providing
		/// exception information about the thread termination. 
		/// Calling this method usually terminates the thread.
		/// </summary>
		/// <param name="stateInfo">An object that contains application-specific information, such as state, which can be used by the thread being aborted</param>
		public void Abort(System.Object stateInfo)
		{
			lock(this)
			{
				threadField.Abort(stateInfo);
			}
		}
	      
		/// <summary>
		/// Suspends the thread, if the thread is already suspended it has no effect
		/// </summary>
		public void Suspend()
		{
			threadField.Suspend();
		}
	      
		/// <summary>
		/// Obtain a String that represents the current Object
		/// </summary>
		/// <returns>A String that represents the current Object</returns>
		public override System.String ToString()
		{
			return "Thread[" + Name + "," + Priority.ToString() + "," + "" + "]";
		}
	     
		/// <summary>
		/// Gets the currently running thread
		/// </summary>
		/// <returns>The currently running thread</returns>
		public static ThreadClass Current()
		{
			ThreadClass CurrentThread = new ThreadClass();
			CurrentThread.Instance = System.Threading.Thread.CurrentThread;
			return CurrentThread;
		}
	}


	/*******************************/
	/// <summary>
	/// Lets you create an action to be performed with security privileges.
	/// </summary>
	public interface IPriviligedAction
	{
		/// <summary>
		/// Performs the priviliged action.
		/// </summary>
		/// <returns>A value that may represent the result of the action.</returns>
		System.Object Run();
	}


	/*******************************/
	/// <summary>
	/// Method used to obtain the underlying type of an object to make the correct property call.
	/// The method is used when getting values from a property.
	/// </summary>
	/// <param name="tempObject">Object instance received</param>
	/// <param name="propertyName">Property name to obtain value</param>
	/// <returns>The return value of the property</returns>
	public static System.Object GetPropertyAsVirtual(System.Object tempObject, System.String propertyName)
	{
		System.Type type = tempObject.GetType();
		System.Reflection.PropertyInfo propertyInfo = type.GetProperty(propertyName);
		try
		{
			return propertyInfo.GetValue(tempObject, null);
		}
		catch(Exception e)
		{
			throw e.InnerException;
		}
	}


}
