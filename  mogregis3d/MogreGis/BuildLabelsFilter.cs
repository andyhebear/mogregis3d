using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sharp3D.Math.Core;

namespace MogreGis
{
    /**
     * Builds osgText labels for features. (UNDER CONSTRUCTION)
     *
     * This filter is still under construction!
     */
    public class BuildLabelsFilter : BuildGeomFilter
    {
        //TODO OSGGIS_META_FILTER( BuildLabelsFilter );


        /**
         * Constructs a new filter for converting features into labels.
         */
        public BuildLabelsFilter()
        {
            setTextScript(new Script("default", "lua", "'text'"));
            setDisableDepthTest(DEFAULT_DISABLE_DEPTH_TEST);
            setFontName(DEFAULT_FONT_NAME);
        }

        public BuildLabelsFilter(BuildLabelsFilter rhs)
            : base(rhs)
        {
            text_script = rhs.text_script;
            font_size_script = rhs.font_size_script;
            disable_depth_test = rhs.disable_depth_test;
            font_name = rhs.font_name;
        }


        //properties

        /**
         * Sets the script that evaluates to the label text.
         * @param script
         *      Script that generates text
         */
        public void setTextScript(Script script)
        {
            text_script = script;
        }

        /**
         * Gets the script that evaluates to the label text.
         * @return
         *      Script that generates text
         */
        public Script getTextScript()
        {
            return text_script;
        }

        /**
         * Sets the script that evaluates to a font size for the text.
         * @param script
         *      Script that generates a font size
         */
        public void setFontSizeScript(Script script)
        {
            font_size_script = script;
        }

        /**
         * Gets the script that evaluates to a font size for the text.
         * @return
         *      Script that generates a font size
         */
        public Script getFontSizeScript()
        {
            return font_size_script.get();
        }

        /**
         * Sets the name of the font to use.
         *
         * @param font_name
         *      Name of the font file to use, e.g. "arial.ttf"
         */
        public void setFontName(string font_name)
        {
            font_name = font_name;
        }

        /**
         * Gets the name of the font to use.
         *
         * @return Font file name, e.g. "arial.ttf"
         */
        public string getFontName()
        {
            return font_name;
        }

        /**
         * Sets a script that evalutes to the text color.
         *
         * @param script
         *      Foreground color for text
         */
        public void setTextColorScript(Script script);

        /**
         * Gets the script that evaluates to the text color (vec4)
         *
         * @return A script
         */
        public Script getTextColorScript();

        /**
         * Sets whether to disable depth testing on the text label geometry.
         * @param value
         *      True to turn off depth buffer testing, false otherwise
         */
        public void setDisableDepthTest(bool value)
        {
            disable_depth_test = value;
        }

        /**
         * Gets whether to disable depth testing on the text label geometry.
         */
        public bool getDisableDepthTest()
        {
            return disable_depth_test;
        }


        // Filter overrides    
        public virtual void setProperty(Property p)
        {
            if (p.getName() == "text")
                setTextScript(new Script(p.getValue()));
            else if (p.getName() == "disable_depth_test")
                setDisableDepthTest(p.getBoolValue(getDisableDepthTest()));
            else if (p.getName() == "font_size")
                setFontSizeScript(new Script(p.getValue()));
            else if (p.getName() == "font")
                setFontName(p.getValue());
            base.setProperty(p);
        }

        public virtual Properties getProperties()
        {
            Properties p = base.getProperties();
            if (getTextScript() != null)
                p.Add(new Property("text", getTextScript().getCode()));
            if (getFontSizeScript() != null)
                p.Add(new Property("font_size", getFontSizeScript().getCode()));
            if (getFontName() != DEFAULT_FONT_NAME)
                p.Add(new Property("font", getFontName()));
            if (getDisableDepthTest() != DEFAULT_DISABLE_DEPTH_TEST)
                p.Add(new Property("disable_depth_test", getDisableDepthTest()));
            return p;
        }

        // FragmentFilter overrides
        protected virtual FragmentList process(FeatureList input, FilterEnv env)
        {
            // load and cache the font
            if (!string.IsNullOrEmpty(getFontName()) && !font.valid())
            {
                font = osgText.readFontFile(getFontName());
            }

            return base.process(input, env);
        }
        protected virtual FragmentList process(Feature input, FilterEnv env)
        {
            FragmentList output;

            // the text string:
            string text;
            if (getTextScript() != null)
            {
                ScriptResult r = env.getScriptEngine().run(getTextScript(), input, env);
                if (r.isValid())
                    text = r.asString();
                else
                    env.getReport().error(r.asString());
            }

            // resolve the size:
            double font_size = 16.0;
            if (getFontSizeScript() != null)
            {
                ScriptResult r = env.getScriptEngine().run(getFontSizeScript(), input, env);
                if (r.isValid())
                    font_size = r.asDouble(font_size);
                else
                    env.getReport().error(r.asString());
            }

            // the text color:
            Vector4D color = getColorForFeature(input, env);

            // calculate the 3D centroid of the feature:
            // TODO: move this to the geoshapelist class
            Vector3D point = new Vector3D(input.getExtent().getCentroid());
            ZCalc zc;
            input.getShapes().accept(zc);
            point.z() = zc.z_count > 0 ? zc.z_sum / (double)zc.z_count : 0.0;

            // build the drawable:
            osgText.Text t = new osgText.Text();
            t.setAutoRotateToScreen(true);
            t.setCharacterSizeMode(osgText.Text.SCREEN_COORDS);
            t.setAlignment(osgText.Text.CENTER_CENTER);
            t.setText(text.c_str());
            t.setColor(color);
            t.setCharacterSize((float)font_size);
            t.setPosition(point);
            t.setBackdropType(osgText.Text.OUTLINE);
            t.setBackdropColor(osg.Vec4(0, 0, 0, 1));

#if PENDING
    // testing the flat-label approach here:
    osg.Matrix cell2map = env.getInputSRS().getInverseReferenceFrame();
    osg.Vec3d feature_center = point * cell2map;
    feature_center.normalize();
    osg.Vec3d cell_center = osg.Vec3d(0,0,1) * cell2map;
    cell_center.normalize();
    osg.Quat q;
    q.makeRotate( cell_center, feature_center );
    t.setRotation( q );
    t.setAutoRotateToScreen( false );
    // end of flat label approach
#endif

            if (font.valid())
            {
                t.setFont(font.get());
            }

            if (getDisableDepthTest())
            {
                t.getOrCreateStateSet().setAttribute(new osg.Depth(osg.Depth.ALWAYS, 0, 1, false), osg.StateAttribute.ON);
            }

            output.Add(new Fragment(t));

            return output;
        }


        private Script text_script;
        private Script font_size_script;
        private Script text_color_script;
        private string font_name;
        private bool disable_depth_test;

        //transient
        private osgText.Font font;

        //TODO  OSGGIS_DEFINE_FILTER( BuildLabelsFilter );
        private const bool DEFAULT_DISABLE_DEPTH_TEST = true;
        private const string DEFAULT_FONT_NAME = "";

        internal class ZCalc : GeoPointVisitor
        {
            public double z_sum = 0.0;
            public int z_count = 0;
            public bool visitPoint(GeoPoint point)
            {
                z_sum += point.getDim() > 2 ? point.Z : 0.0;
                z_count++;
                return true;
            }
        }

    }
}
