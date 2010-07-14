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
using System.Collections.Generic;
using Filter = scenic.filter.Filter;
using Path = scenic.path.Path;
using PathBuilder = scenic.path.PathBuilder;
namespace scenic
{
    public abstract class SceneNode
    {
        internal abstract void draw(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ref System.Drawing.Rectangle visibleArea);
        internal virtual void prepareDraw(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
        }

        internal virtual int getDrawType(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
            return 0;
        }

        internal abstract System.Drawing.Rectangle getBounds(DrawContext context, System.Drawing.Drawing2D.Matrix transform);
        protected internal virtual void show()
        {
        }
        protected internal virtual void hide()
        {
        }
    }


    /// <summary> ScenicGraphics class offers an easy to use interface for creating scene
    /// trees. Instead of creating scene graphs directly, Graphics class
    /// has a more traditional method-based interface. The Graphics class
    /// transforms the method calls into appropriate scene graphs. Paths can be
    /// created using methods that create lines, curves, arcs and rectangles
    /// These paths can then be either stroked,
    /// filled or clipped to. Text can also be drawn using simple method calls.
    /// The current affine transformation can easily be changed using different
    /// methods which translate, Scale or Rotate the current transformation.
    /// The Graphics object also has a stack which can be used to store and restore 
    /// the state of the Graphics object.
    /// </summary>
    public class Graphics
    {
        /// <summary> Gets the total transformation of the current scene node.
        /// 
        /// </summary>
        /// <returns> the total transformation.
        /// </returns>
        virtual public System.Drawing.Drawing2D.Matrix Transform
        {
            get
            {
                return state.transform;
            }

        }
        /// <summary> Gets the square root of the area of a 1 by 1 Rectangle 
        /// when transformed into screen coordinates.
        /// 
        /// </summary>
        /// <returns> the pixel size.
        /// </returns>
        virtual public double PixelSize
        {
            get
            {
                //UPGRADE_WARNING: At least one expression was used more than once in the target code. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1181_3"'
                Transform.Elements.GetValue(0);
                Transform.Elements.GetValue(3);
                Transform.Elements.GetValue(2);
                return System.Math.Sqrt((float)Transform.Elements.GetValue(1));
            }

        }
        /// <summary> Sets the color that is used for stroking paths.
        /// 
        /// </summary>
        /// <param name="c">Stroke color.
        /// </param>
        virtual public ScenicColor StrokeColor
        {
            set
            {
                state.strokeColor = value;
                state.strokeBrush = null;
            }

        }
        /// <summary> Sets the brush used for stroking paths.
        /// 
        /// </summary>
        /// <param name="brush">brush used for stroking.
        /// </param>
        virtual public SceneNode StrokeBrush
        {
            set
            {
                state.strokeColor = null;
                state.strokeBrush = value;
            }

        }
        /// <summary> Sets the color that is used for filling paths and drawing text.
        /// 
        /// </summary>
        /// <param name="c">Fill color.
        /// </param>
        virtual public ScenicColor FillColor
        {
            set
            {
                state.fillColor = value;
                state.fillBrush = null;
            }

        }
        /// <summary> Sets the brush used for filling paths and drawing text.
        /// 
        /// </summary>
        /// <param name="brush">brush used for filling.
        /// </param>
        virtual public SceneNode FillBrush
        {
            set
            {
                state.fillColor = null;
                state.fillBrush = value;
            }

        }
        /// <summary> Sets the current font.
        /// 
        /// </summary>
        /// <param name="font">the font.
        /// </param>
        virtual public System.Drawing.Font Font
        {
            set
            {
                state.font = value;
            }

        }
        /// <summary> Sets the line width. The line width is given in logical coordinates.
        /// 
        /// </summary>
        /// <param name="width">the line width.
        /// </param>
        virtual public float LineWidth
        {
            set
            {
                state.lineWidth = value;
            }

        }
        /// <summary> Sets the line cap style. The line cap style determines how
        /// the ends of paths are drawn.
        /// 
        /// </summary>
        /// <param name="lineCap">the line cap style.
        /// </param>
        virtual public LineCapStyle EndCap
        {
            set
            {
                state.lineCap = value;
            }

        }
        /// <summary> Sets the line join style. The line join style determines how
        /// joints between lines are drawn.
        /// 
        /// </summary>
        /// <param name="lineJoin">the line join style.
        /// </param>
        virtual public LineJoinStyle LineJoin
        {
            set
            {
                state.lineJoin = value;
            }

        }

        /// <summary> Sets the miter limit. The miter limit determines the cutoff length
        /// of the spikes when using miter join.
        /// 
        /// </summary>
        /// <param name="miterLimit">the miter limit.
        /// </param>
        virtual public float MiterLimit
        {
            set
            {
                state.miterLimit = value;
            }

        }

        /// <summary> Sets the line dash pattern. The dash pattern is defined
        /// using an array which contains the lengths of consecutive
        /// visible and non-visible portions of the dash pattern. The 
        /// lenths are given in logical units. 
        /// 
        /// </summary>
        /// <param name="lineDashLengths">the line dash pattern.
        /// </param>
        virtual public float[] DashArray
        {
            set
            {
                state.lineDashLengths = value;
            }

        }
        /// <summary> Sets the phase of the line dash pattern. The phase defines the
        /// starting position of the line dash pattern.
        /// 
        /// </summary>
        /// <param name="lineDashPhase">the line dash phase.
        /// </param>
        virtual public float DashPhase
        {
            set
            {
                state.lineDashPhase = value;
            }

        }

        /// <summary> Sets antialiasing. The antialiasing affects all graphics, including
        /// lines, polygons an text.
        /// 
        /// </summary>
        /// <param name="aa">the antialiasing setting.
        /// </param>
        virtual public bool Antialias
        {
            set
            {
                AntialiasingFilter = value ? SceneSettings.DefaultAAFilter : null;
            }

        }

        /// <summary> Sets the filter that is used for antialiasing.
        /// 
        /// </summary>
        /// <param name="filter">antialiasing filter
        /// </param>
        virtual public Filter AntialiasingFilter
        {
            set
            {
                SceneSettings settings = new SceneSettings();

                settings.AntialiasingFilter = value;
                state.scene.add(settings);
                state.scene = settings;
            }

        }
        /// <summary> Sets whatever fractional metrics are used to calculate glyph bounds.
        /// This parameter affects only the positioning of characters but
        /// does not change their appearance. This parameter also affects
        /// the bounds returned by getTextLogialBounds.
        /// 
        /// </summary>
        /// <param name="b">the fractional metrics setting.
        /// </param>
        virtual public bool UsesFractionalFontMetrics
        {
            set
            {
                state.usesFractionalFontMetrics = value;
            }

        }
        /// <summary> Gets a Path object that contains the current path.
        /// 
        /// </summary>
        /// <returns> the current path.
        /// </returns>
        virtual public Path Path
        {
            get
            {
                return path.createPath();
            }

        }

        private class State : System.ICloneable
        {
            public SceneContainer scene;
            public System.Drawing.Drawing2D.Matrix transform;
            public System.Drawing.Font font;
            public ScenicColor strokeColor;
            public SceneNode strokeBrush;
            public ScenicColor fillColor;
            public SceneNode fillBrush;
            public float lineWidth;
            public LineCapStyle lineCap;
            public LineJoinStyle lineJoin;
            public float miterLimit = 10.0f;
            public float[] lineDashLengths;
            public float lineDashPhase;
            public FillRule fillRule;
            public float[] textPosition;
            public bool usesFractionalFontMetrics;
            public Render.IRendererCallback callbacks;

            public State()
            {
                textPosition = new float[2];
                this.transform = new System.Drawing.Drawing2D.Matrix();
            }

            public virtual System.Object Clone()
            {
                try
                {
                    State s = (State)base.MemberwiseClone();

                    s.textPosition = new float[textPosition.Length];
                    textPosition.CopyTo(s.textPosition, 0);
                    //UPGRADE_TODO: Method 'java.awt.geom.AffineTransform.clone' was converted to 'System.Drawing.Drawing2D.Matrix.Clone' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtgeomAffineTransformclone_3"'
                    s.transform = (System.Drawing.Drawing2D.Matrix)transform.Clone();

                    return s;
                }
                //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100_3"'
                catch (System.Exception e)
                {
                    Console.WriteLine("Error " + e);
                    return null;
                }
            }

            public virtual void set_Renamed(State state)
            {
                this.scene = state.scene;
                //UPGRADE_ISSUE: Method 'java.awt.geom.AffineTransform.setTransform' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomAffineTransformsetTransform_javaawtgeomAffineTransform_3"'
                //TODO PENDING this.transform.setTransform(state.transform);
                this.font = state.font;
                this.strokeColor = state.strokeColor;
                this.fillColor = state.fillColor;
                this.fillBrush = state.fillBrush;
                this.lineWidth = state.lineWidth;
                this.lineCap = state.lineCap;
                this.lineJoin = state.lineJoin;
                this.miterLimit = state.miterLimit;
                this.lineDashLengths = state.lineDashLengths;
                this.lineDashPhase = state.lineDashPhase;
                this.fillRule = state.fillRule;
                this.textPosition = state.textPosition;
                this.usesFractionalFontMetrics = state.usesFractionalFontMetrics;
            }
        }


        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
        private Stack<State> statePool = new Stack<State>();

        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
        private Stack<State> stateStack;
        private State state;
        private PathBuilder path;

        /// <summary> Constructs a new Graphics object using the given SceneContainer as
        /// the root node. The transformation matrix of the root node is also
        /// given. This matrix is used to position hinted glyphs properly.
        /// 
        /// </summary>
        /// <param name="scene">The root node into which graphics is drawn.
        /// </param>
        /// <param name="transform">The transformation matrix of the root node.
        /// </param>
        public Graphics(SceneContainer scene, System.Drawing.Drawing2D.Matrix transform)
        {
            //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
            stateStack = new Stack<State>();
            state = newState();
            state.scene = scene;
            //UPGRADE_TODO: Method 'java.awt.geom.AffineTransform.clone' was converted to 'System.Drawing.Drawing2D.Matrix.Clone' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtgeomAffineTransformclone_3"'
            state.transform = (System.Drawing.Drawing2D.Matrix)transform.Clone();
            //UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1075_3"'
            //UPGRADE_TODO: Field 'java.awt.Font.PLAIN' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtFontPLAIN_f_3"'
            state.font = new System.Drawing.Font("SansSerif", 12, System.Drawing.FontStyle.Regular);
            state.strokeColor = new ScenicColor(0, 0, 0);
            state.fillColor = state.strokeColor;
            state.lineWidth = 1.0f;
            state.lineCap = LineCapStyle.BUTT_CAP;
            state.lineJoin = LineJoinStyle.BEVEL_JOIN;
            state.miterLimit = 10.0f;
            state.lineDashLengths = null;
            state.lineDashPhase = 0.0f;
            state.fillRule = FillRule.ODD_WINDING;
            state.usesFractionalFontMetrics = false;
            path = new PathBuilder();
        }

        /// <summary> Constructs a new Graphics object using the given SceneContainer as
        /// the root node.
        /// 
        /// </summary>
        /// <param name="scene">The root node into which graphics is drawn.
        /// </param>
        public Graphics(SceneContainer scene)
            : this(scene, new System.Drawing.Drawing2D.Matrix())
        {
        }

        public Graphics(Render.IRendererCallback rendererClbks) //TODO PENDING
        {
            stateStack = new Stack<State>();
            state = new State();
            state.scene = new SceneContainer();
            path = new PathBuilder();
            if (rendererClbks == null)
                state.callbacks = new Render.VertexStore();
            else
                state.callbacks = rendererClbks;
        }

        /// <summary> Construct a new ScenicGraphics object that inherits the
        /// state of the given ScenicGraphics object.
        /// 
        /// </summary>
        /// <param name="parent">the parent object
        /// </param>
        public Graphics(Graphics parent)
        {
            stateStack = new Stack<State>();
            state = (State)parent.state.Clone();
            state.scene = new SceneContainer();
            parent.state.scene.add(state.scene);
            path = new PathBuilder();
        }

        private State newState()
        {
            if (statePool.Count != 0)
                return statePool.Pop();
            return new State();
        }

        private State cloneState(State state)
        {
            State s = newState();

            s.set_Renamed(state);
            return s;
        }

        /// <summary> Creates a copy of this graphics object.
        /// 
        /// </summary>
        /// <returns> the copy
        /// </returns>
        public virtual Graphics create()
        {
            return new Graphics(this);
        }

        /// <summary> Clears the current scene.</summary>
        public virtual void Clear()
        {
            state.scene.clear();
        }

        /// <summary> Pushes the state of this object into the stack. The state includes
        /// the entire internal state of the Graphics object excluding the 
        /// current path.
        /// </summary>
        public virtual void Push()
        {
            stateStack.Push(state);
            state = cloneState(state);
        }

        /// <summary> Pops the state from the stack.</summary>
        public virtual void Pop()
        {
            statePool.Push(state);
            state = stateStack.Pop();
        }

        /// <summary> Multiplies the current transformation matrix with the
        /// given affine transform.
        /// 
        /// </summary>
        /// <param name="m">the affine transform
        /// </param>
        public virtual void transform(System.Drawing.Drawing2D.Matrix m)
        {
            //UPGRADE_TODO: Method 'java.awt.geom.AffineTransform.clone' was converted to 'System.Drawing.Drawing2D.Matrix.Clone' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtgeomAffineTransformclone_3"'
            transformImpl((System.Drawing.Drawing2D.Matrix)m.Clone());
        }

        private void transformImpl(System.Drawing.Drawing2D.Matrix m)
        {
            SceneTransform st = new SceneTransform(m);

            state.scene.add(st);
            state.scene = st;
            //UPGRADE_TODO: Method 'java.awt.geom.AffineTransform.concatenate' was converted to 'System.Drawing.Drawing2D.Matrix.Multiply' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtgeomAffineTransformconcatenate_javaawtgeomAffineTransform_3"'
            state.transform.Multiply(m, System.Drawing.Drawing2D.MatrixOrder.Append);
        }

        /// <summary> Translates the current transformation matrix by the given
        /// displacement.
        /// 
        /// </summary>
        /// <param name="dx">x coordinate of the translation
        /// </param>
        /// <param name="dy">y coordinate of the translation
        /// </param>
        public virtual void translate(double dx, double dy)
        {
            System.Drawing.Drawing2D.Matrix temp_Matrix;
            temp_Matrix = new System.Drawing.Drawing2D.Matrix();
            temp_Matrix.Translate((float)dx, (float)dy);
            transform(temp_Matrix);
        }

        /// <summary> Scales the current transformation matrix by the given
        /// amount.
        /// 
        /// </summary>
        /// <param name="x">x coordinate of the Scale factor
        /// </param>
        /// <param name="y">y coordinate of the Scale factor
        /// </param>
        public virtual void Scale(double x, double y)
        {
            System.Drawing.Drawing2D.Matrix temp_Matrix;
            temp_Matrix = new System.Drawing.Drawing2D.Matrix();
            temp_Matrix.Scale((float)x, (float)y);
            transform(temp_Matrix);
        }

        /// <summary> Rotates the current transformation by the given angle.
        /// 
        /// </summary>
        /// <param name="angle">rotation in degrees.
        /// </param>
        public virtual void Rotate(double angle)
        {
            System.Drawing.Drawing2D.Matrix temp_Matrix;
            temp_Matrix = new System.Drawing.Drawing2D.Matrix();
            temp_Matrix.Rotate((float)(SupportClass.DegreesToRadians(angle) * (180 / System.Math.PI)));
            transform(temp_Matrix);
        }

        /// <summary> Rotates the current transformation around the given
        /// pivot point by the given angle.
        /// 
        /// </summary>
        /// <param name="angle">rotation in degrees.
        /// </param>
        /// <param name="pivotX">x coordinate of the pivot point.
        /// </param>
        /// <param name="pivotY">y coordinate of the pivot point.
        /// </param>
        public virtual void Rotate(double angle, double pivotX, double pivotY)
        {
            System.Drawing.Drawing2D.Matrix temp_Matrix;
            temp_Matrix = new System.Drawing.Drawing2D.Matrix();
            temp_Matrix.RotateAt((float)(SupportClass.DegreesToRadians(angle) * (180 / System.Math.PI)), new System.Drawing.PointF((float)pivotX, (float)pivotY));
            transform(temp_Matrix);
        }

        /// <summary> Shears the current transformation matrix by the given
        /// amount.
        /// 
        /// </summary>
        /// <param name="x">x coordinate of the shear factor
        /// </param>
        /// <param name="y">y coordinate of the shear factor
        /// </param>
        public virtual void shear(double x, double y)
        {
            System.Drawing.Drawing2D.Matrix temp_Matrix;
            temp_Matrix = new System.Drawing.Drawing2D.Matrix();
            temp_Matrix.Shear((float)x, (float)y);
            transform(temp_Matrix);
        }

        /// <summary> Sets the text position. The text position defines the start point
        /// for drawing text in logical coordinates.
        /// 
        /// </summary>
        /// <param name="x">x coordinate of the text position.
        /// </param>
        /// <param name="y">y coordinate of the text position.
        /// </param>
        public virtual void setTextPosition(float x, float y)
        {
            state.textPosition[0] = x;
            state.textPosition[1] = y;
        }

        /// <summary> Draws the given text using the current font, Fill color and
        /// other settings.
        /// 
        /// </summary>
        /// <param name="text">the text to be drawn.
        /// </param>
        public virtual void drawText(System.String text)
        {
#if PENDING
            float[] pos;
            //UPGRADE_ISSUE: Class 'java.awt.font.GlyphVector' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontGlyphVector_3"'
            GlyphVector gv;
            //UPGRADE_TODO: Class 'java.awt.font.FontRenderContext' was converted to 'System.Windows.Forms.Control' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_3"'
            //UPGRADE_ISSUE: Constructor 'java.awt.font.FontRenderContext.FontRenderContext' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontFontRenderContextFontRenderContext_javaawtgeomAffineTransform_boolean_boolean_3"'
            System.Drawing.Text.TextRenderingHint frc = new FontRenderContext(state.transform, true, state.usesFractionalFontMetrics);

            //UPGRADE_TODO: Method 'java.awt.Font.createGlyphVector' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095_3"'
            gv = state.font.createGlyphVector(frc, text);
            //UPGRADE_ISSUE: Method 'java.awt.font.GlyphVector.getGlyphPositions' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontGlyphVector_3"'
            //UPGRADE_ISSUE: Method 'java.awt.font.GlyphVector.getNumGlyphs' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontGlyphVector_3"'
            pos = gv.getGlyphPositions(0, gv.getNumGlyphs(), null);

            //UPGRADE_ISSUE: Method 'java.awt.font.GlyphVector.getGlyphCodes' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontGlyphVector_3"'
            //UPGRADE_ISSUE: Method 'java.awt.font.GlyphVector.getNumGlyphs' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontGlyphVector_3"'
            drawGlyphs(gv.getGlyphCodes(0, gv.getNumGlyphs(), null), pos);
#endif
        }

        /// <summary> Draws the given text using the current font, Fill color and
        /// other settings. The positions of the characters are given 
        /// as an array of floats. Each position is given as two
        /// numbers the x-position and y-position.
        /// 
        /// </summary>
        /// <param name="text">the text to be drawn.
        /// </param>
        /// <param name="positions">the positions of the characters.
        /// </param>
        public virtual void drawText(System.String text, float[] positions)
        {
#if PENDING
            //UPGRADE_TODO: Class 'java.awt.font.FontRenderContext' was converted to 'System.Windows.Forms.Control' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_3"'
            //UPGRADE_ISSUE: Constructor 'java.awt.font.FontRenderContext.FontRenderContext' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontFontRenderContextFontRenderContext_javaawtgeomAffineTransform_boolean_boolean_3"'
            System.Drawing.Text.TextRenderingHint frc = new FontRenderContext(new System.Drawing.Drawing2D.Matrix(), true, true);
            //UPGRADE_ISSUE: Class 'java.awt.font.GlyphVector' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontGlyphVector_3"'
            //UPGRADE_TODO: Method 'java.awt.Font.createGlyphVector' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095_3"'
            GlyphVector gv = state.font.createGlyphVector(frc, text);

            //UPGRADE_ISSUE: Method 'java.awt.font.GlyphVector.getGlyphCodes' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontGlyphVector_3"'
            //UPGRADE_ISSUE: Method 'java.awt.font.GlyphVector.getNumGlyphs' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontGlyphVector_3"'
            drawGlyphs(gv.getGlyphCodes(0, gv.getNumGlyphs(), null), positions);
#endif
        }

        /// <summary> Draws the given glyphs using the current font, Fill color and
        /// other settings. The positions of the glyphs are given 
        /// as an array of floats. Each position is given as two
        /// numbers the x-position and y-position.
        /// 
        /// </summary>
        /// <param name="glyphCodes">the glyph codes.
        /// </param>
        /// <param name="positions">the positions of the characters.
        /// </param>
        public virtual void drawGlyphs(int[] glyphCodes, float[] positions)
        {
            System.Drawing.Drawing2D.Matrix temp_Matrix;
            temp_Matrix = new System.Drawing.Drawing2D.Matrix();
            temp_Matrix.Translate((float)state.textPosition[0], (float)state.textPosition[1]);
            SceneTransform st = new SceneTransform(temp_Matrix);
            TextShape textShape = new TextShape(state.font, glyphCodes, positions);

            state.scene.add(st);

            if (state.fillColor != null)
            {
                textShape.Color = state.fillColor;
                st.add(textShape);
            }
            else
            {
                SceneClip sc = new SceneClip(textShape);

                sc.add(state.fillBrush);
                st.add(sc);
            }
        }

        /// <summary> Returns the logical bounds of the given text.
        /// 
        /// </summary>
        /// <param name="text">the text.
        /// </param>
        /// <returns> the logical bounds of the text.
        /// </returns>
        public virtual System.Drawing.RectangleF getTextLogicalBounds(System.String text)
        {
#if PENDING
            //UPGRADE_TODO: Class 'java.awt.font.FontRenderContext' was converted to 'System.Windows.Forms.Control' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_3"'
            //UPGRADE_ISSUE: Constructor 'java.awt.font.FontRenderContext.FontRenderContext' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontFontRenderContextFontRenderContext_javaawtgeomAffineTransform_boolean_boolean_3"'
            System.Drawing.Text.TextRenderingHint frc = new FontRenderContext(new System.Drawing.Drawing2D.Matrix(), true, state.usesFractionalFontMetrics);
            //UPGRADE_ISSUE: Class 'java.awt.font.GlyphVector' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontGlyphVector_3"'
            //UPGRADE_TODO: Method 'java.awt.Font.createGlyphVector' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095_3"'
            GlyphVector gv = state.font.createGlyphVector(frc, text);

            //UPGRADE_ISSUE: Method 'java.awt.font.GlyphVector.getLogicalBounds' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontGlyphVector_3"'
            return gv.getLogicalBounds();
#endif
            return new System.Drawing.RectangleF(); //TODO PENDING
        }

        /// <summary> Begins a new subpath from the given position.</summary>
        public virtual void MoveTo(float x, float y)
        {
            path.moveTo(x, y);
        }

        /// <summary> Draws a straight line to the given position.</summary>
        public virtual void LineTo(float x, float y)
        {
            path.lineTo(x, y);
        }

        /// <summary> Draws a quadratic Bezier-curve using the given
        /// control points. The first control point is the current
        /// position.
        /// 
        /// </summary>
        /// <param name="p1">the second control point.
        /// </param>
        /// <param name="p2">the third control point.
        /// </param>
        public virtual void CurveTo(System.Drawing.PointF p1, System.Drawing.PointF p2)
        {
            path.curveTo(p1, p2);
        }

        /// <summary> Draws a cubic Bezier-curve using the given
        /// control points. The first control point is the current
        /// position.
        /// 
        /// </summary>
        /// <param name="p1">the second control point.
        /// </param>
        /// <param name="p2">the third control point.
        /// </param>
        /// <param name="p3">the fourth control point.
        /// </param>
        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
        public virtual void CurveTo(ref System.Drawing.PointF p1, ref System.Drawing.PointF p2, ref System.Drawing.PointF p3)
        {
            //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
            path.curveTo(ref p1, ref p2, ref p3);
        }

        /// <summary> Draws an elliptic Arc starting from the current position.
        /// 
        /// </summary>
        /// <param name="xRadius">the x radius of the ellipse.
        /// </param>
        /// <param name="yRadius">the y radius of the ellipse.
        /// </param>
        /// <param name="rotation">the rotation of the ellipse in degrees.
        /// </param>
        /// <param name="startAngle">the start angle of the Arc in degrees.
        /// </param>
        /// <param name="stopAngle">the stop angle of the Arc in degrees.
        /// </param>
        public virtual void ArcTo(double xRadius, double yRadius, double rotation, double startAngle, double stopAngle)
        {
            path.arcTo(xRadius, yRadius, rotation, startAngle, stopAngle);
        }

        /// <summary> Draws an elliptic Arc starting from the given position.
        /// 
        /// </summary>
        /// <param name="x">the x-coordinate of the starting point of the Arc.
        /// </param>
        /// <param name="y">the y-coordinate of the starting point of the Arc.
        /// </param>
        /// <param name="xRadius">the x radius of the ellipse.
        /// </param>
        /// <param name="yRadius">the y radius of the ellipse.
        /// </param>
        /// <param name="rotation">the rotation of the ellipse in degrees.
        /// </param>
        /// <param name="startAngle">the start angle of the Arc in degrees.
        /// </param>
        /// <param name="stopAngle">the stop angle of the Arc in degrees.
        /// </param>
        public virtual void Arc(float x, float y, double xRadius, double yRadius, double rotation, double startAngle, double stopAngle)
        {
            path.arc(x, y, xRadius, yRadius, rotation, startAngle, stopAngle);
        }

        /// <summary> Draws a Rectangle.
        /// 
        /// </summary>
        /// <param name="x">the left side of the Rectangle.
        /// </param>
        /// <param name="y">the top of the Rectangle.
        /// </param>
        /// <param name="width">the width of the Rectangle.
        /// </param>
        /// <param name="height">the height of the Rectangle.
        /// </param>
        public virtual void Rectangle(float x, float y, float width, float height)
        {
            path.rectangle(x, y, width, height);
        }

        /// <summary> Draws a Rectangle with rounded edges. 
        /// 
        /// </summary>
        /// <param name="x">the left side of the Rectangle.
        /// </param>
        /// <param name="y">the top of the Rectangle.
        /// </param>
        /// <param name="width">the width of the Rectangle.
        /// </param>
        /// <param name="height">the height of the Rectangle.
        /// </param>
        /// <param name="radius">the radius of the rounded edges.
        /// </param>
        public virtual void RoundedRectangle(float x, float y, float width, float height, float radius)
        {
            path.roundedRectangle(x, y, width, height, radius);
        }

        /// <summary> Closes the current subpath.</summary>
        public virtual void Close()
        {
            path.close();
        }

        private SceneNode createNode(SceneShape shape, ScenicColor color, SceneNode brush)
        {
            if (color != null)
            {
                shape.Color = color;

                return shape;
            }
            else
            {
                SceneClip sc = new SceneClip(shape);

                sc.add(brush);

                return sc;
            }
        }

        /// <summary> Strokes the current path.</summary>
        public virtual void Stroke()
        {
            StrokedPath r = new StrokedPath(path.createPath());

            r.LineWidth = state.lineWidth;
            r.EndCap = state.lineCap;
            r.LineJoin = state.lineJoin;
            r.MiterLimit = state.miterLimit;
            r.DashArray = state.lineDashLengths;
            r.DashPhase = state.lineDashPhase;

            SceneNode node = createNode(r, state.strokeColor, state.strokeBrush);
            state.scene.add(node);

            System.Drawing.Rectangle rec = new System.Drawing.Rectangle();
            path = new PathBuilder();

            state.scene.draw(new DrawContext(state.callbacks), state.transform, ref rec);

        }


        /// <summary> Fills the current path.</summary>
        public virtual void Fill()
        {
            FilledPath shape = new FilledPath(path.createPath());

            shape.FillRule = state.fillRule;
            state.scene.add(createNode(shape, state.fillColor, state.fillBrush));

            path = new PathBuilder();
        }

        /// <summary> Fills and strokes the current path.</summary>
        public virtual void FillAndStroke()
        {
            Path p = path.createPath();
            FilledPath shape = new FilledPath(p);

            shape.FillRule = state.fillRule;
            state.scene.add(createNode(shape, state.fillColor, state.fillBrush));

            StrokedPath r2 = new StrokedPath(p);

            r2.LineWidth = state.lineWidth;
            r2.EndCap = state.lineCap;
            r2.LineJoin = state.lineJoin;
            r2.MiterLimit = state.miterLimit;

            state.scene.add(createNode(r2, state.strokeColor, state.strokeBrush));

            path = new PathBuilder();
        }

        /// <summary> Clips to the area inside the current path.</summary>
        public virtual void Clip()
        {
            FilledPath shape = new FilledPath(path.createPath());
            SceneClip clip = new SceneClip(shape);

            state.scene.add(clip);
            state.scene = clip;

            path = new PathBuilder();
        }

        /// <summary> Draws an image.
        /// 
        /// </summary>
        /// <param name="img">the image.
        /// </param>
        public virtual void DrawImage(Image img)
        {
            state.scene.add(new SceneImage(img));
        }

        /// <summary> Draws the given part of an image.
        /// 
        /// </summary>
        /// <param name="img">the image.
        /// </param>
        /// <param name="sourceRect">the part of the image that is displayed.
        /// </param>
        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
        public virtual void DrawImage(Image img, ref System.Drawing.RectangleF sourceRect)
        {
            //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
            state.scene.add(new SceneImage(img, ref sourceRect));
        }

        /// <summary> Adds a scene node to the scene.
        /// 
        /// </summary>
        /// <param name="node">the scene node top be added.
        /// </param>
        public virtual void add(SceneNode node)
        {
            state.scene.add(node);
        }

        /// <summary> Adds a scene node to the scene at the given point.
        /// 
        /// </summary>
        /// <param name="node">the scene node top be added
        /// </param>
        /// <param name="x">x coordinate of the point
        /// </param>
        /// <param name="y">y coordinate of the point
        /// </param>
        public virtual void add(SceneNode node, double x, double y)
        {
            System.Drawing.Drawing2D.Matrix temp_Matrix;
            temp_Matrix = new System.Drawing.Drawing2D.Matrix();
            temp_Matrix.Translate((float)x, (float)y);
            SceneTransform s = new SceneTransform(temp_Matrix);

            s.add(node);
            state.scene.add(s);
        }

        /// <summary> Clips graphics using the given Clip scene.
        /// 
        /// </summary>
        /// <param name="clipScene">the scene node that is used for clipping.
        /// </param>
        public virtual void ClipTo(SceneNode clipScene)
        {
            SceneClip clip = new SceneClip(clipScene);

            state.scene.add(clip);
            state.scene = clip;
        }
    }
}