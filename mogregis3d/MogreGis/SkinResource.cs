using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;
using Sharp3D.Math.Core;

namespace MogreGis
{
    /**
     * Resource pointing to an non-geo-referenced image that can be used as a texture map.
     */
    public class SkinResource : Resource
    {
        //TODO OSGGIS_META_RESOURCE(SkinResource);


        /**
         * Constructs a new skin.
         */
        public SkinResource();

        /**
         * Constructs a new skin.
         * @param name
         *      Name of the skin resource
         */
        public SkinResource(string name);

        /**
         * Constructs a new single-use skin directly from an image bitmap.
         *
         * @param image
         *      Image to embed in the resource.
         */
        //SkinResource( osg::Image* image );

        // properties

        /**
         * Sets the real-world width, in meters, of the image.
         *
         * @param value
         *      Tetxture width (in meters)
         */
        public void setTextureWidthMeters(double value);

        /**
         * Gets the real-world width of the image
         *
         * @return Texture width (meters)
         */
        public double getTextureWidthMeters();

        /**
         * Sets the real-world height of the image
         *
         * @param value
         *      Texture height (in meters)
         */
        public void setTextureHeightMeters(double value);

        /**
         * Gets the real-world height of the image
         *
         * @return Texture height (in meters)
         */
        public double getTextureHeightMeters();

        /**
         * Sets the minimum acceptable real-world height for which this
         * image would make an appropriate texture
         *
         * @param value
         *      Texture height (in meters )
         */
        public void setMinTextureHeightMeters(double value);

        /**
         * Gets the minimum acceptable real-world height for which this
         * image would make an appropriate texture
         *
         * @return Texture height (in meters )
         */
        public double getMinTextureHeightMeters();

        /**
         * Sets the maximum acceptable real-world height for which this
         * image would make an appropriate texture
         *
         * @param value
         *      Texture height (in meters )
         */
        public void setMaxTextureHeightMeters(double value);

        /**
         * Gets the maximum acceptable real-world height for which this
         * image would make an appropriate texture
         *
         * @return Texture height (in meters )
         */
        public double getMaxTextureHeightMeters();

        /**
         * Sets whether this is a repeating texture in the Y direction.
         *
         * @param value
         *      True if it repeats; false if not.
         */
        public void setRepeatsVertically(bool value);

        /**
         * Gets whether this is a repeating texture in the Y direction.
         *
         * @return True if it repeats; false if not.
         */
        public bool getRepeatsVertically();

        /**
         * Sets the OpenGL texture mapping mode for the image (default
         * is GL_MODULATE)
         *
         * @param mode
         *      OpenGL mode (GL_DECAL, GL_REPLACE, GL_MODULATE, GL_BLEND)
         */
        public void setTextureMode(osg.TexEnv.Mode mode);

        /**
         * Gets the OpenGL texture mapping mode for the image
         *
         * @return OpenGL mode (GL_DECAL, GL_REPLACE, GL_MODULATE, GL_BLEND)
         */
        public osg.TexEnv.Mode getTextureMode();

        /**
         * Sets the maximum texture size (in either dimension) hint for this
         * skin texture.
         *
         * @param value
         *      Maximum allowable size (0 = unrestricted)
         */
        public void setMaxTextureSize(uint value);

        /**
         * Gets the maximum texture size (in either dimension) hint for this
         * skin texture.
         *
         * @return Max allowable size (0 = unrestricted)
         */
        public uint getMaxTextureSize();


        public void setColor(Vector4D value);
        public Vector4D getColor();



        public Material createStateSet();

        public Material createStateSet(Mogre.Image image);



        public virtual void setProperty(Property prop);
        public virtual Properties getProperties();


        private double tex_width_m;
        private double tex_height_m;
        private double min_tex_height_m;
        private double max_tex_height_m;
        private bool repeats_vertically;
        private osg.TexEnv.Mode texture_mode;
        private uint max_texture_size;
        private Vector4D color;
        //osg::ref_ptr<osg::Image> image;

        private Mogre.Image createImage();

        private void init();
    }

    public class SkinResources : List<SkinResource> { }


    public class SkinResourceQuery
    {

        public SkinResourceQuery();

        public void setTextureHeight(double value);
        public bool hasTextureHeight();
        public double getTextureHeight();

        public void setMinTextureHeight(double value);
        public bool hasMinTextureHeight();
        public double getMinTextureHeight();

        public void setMaxTextureHeight(double value);
        public bool hasMaxTextureHeight();
        public double getMaxTextureHeight();

        public void setRepeatsVertically(bool value);
        public bool hasRepeatsVertically();
        public bool getRepeatsVertically();

        public void addTag(string tag);
        public TagList getTags();

        public string getHashCode();


        private bool has_tex_height;
        private double tex_height;

        private bool has_min_tex_height;
        private double min_tex_height;

        private bool has_max_tex_height;
        private double max_tex_height;

        private bool has_repeat_vert;
        private bool repeat_vert;

        private TagList tags;

        private string hash_code;
    }
}
