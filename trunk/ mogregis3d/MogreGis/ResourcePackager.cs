using System;


namespace MogreGis
{
    /* (todo: refactor this into the osgGISProjects library, along with
     *        all the various layer compilers)
     * Copies referenced resources from the resource library to an output
     * location.
     */
    public class ResourcePackager
    {

        public ResourcePackager()
        {
            compress_textures = DEFAULT_COMPRESS_TEXTURES;
            max_tex_size = DEFAULT_MAX_TEX_SIZE;
            inline_textures = DEFAULT_INLINE_TEXTURES;
            fix_mipmaps = DEFAULT_FIX_MIPMAPS;
        }

        public ResourcePackager clone()
        {
            ResourcePackager copy = new ResourcePackager();
            copy.setArchive(archive.get());
            copy.setOutputLocation(output_location);
            copy.setCompressTextures(compress_textures);
            copy.setMaxTextureSize(max_tex_size);
            copy.setInlineTextures(inline_textures);
            copy.setFixMipmaps(fix_mipmaps);
            return copy;
        }

        public void setArchive(osgDB.Archive value)
        {
            archive = value;
        }

        public void setOutputLocation(string value)
        {
            output_location = value;
        }

        public void setMaxTextureSize(uint value)
        {
            max_tex_size = value;
        }

        public uint getMaxTextureSize()
        {
            return max_tex_size;
        }

        public void setCompressTextures(bool value)
        {
            compress_textures = value;
        }

        public bool getCompressTextures()
        {
            return compress_textures;
        }

        public void setInlineTextures(bool value)
        {
            inline_textures = value;
        }

        public bool getInlineTextures()
        {
            return inline_textures;
        }

        public void setFixMipmaps(bool value)
        {
            fix_mipmaps = value;
        }

        public bool getFixMipmaps()
        {
            return fix_mipmaps;
        }

        public void rewriteResourceReferences(osg.Node node)
        {
            throw new NotImplementedException();
        }

        public void packageResources(ResourceCache resources, Report report)
        {
            throw new NotImplementedException();
        }

        public bool packageNode(osg.Node node, string abs_uri)
        {
            throw new NotImplementedException();
        }

        static void fixMipmapSettings(osg.StateSet ss)
        {
            for (uint i = 0; i < ss.getTextureAttributeList().size(); i++)
            {
                osg.Texture2D tex = (osg.Texture2D)(ss.getTextureAttribute(i, osg.StateAttribute.TEXTURE));
                if (tex != null)
                {
                    tex.setFilter(osg.Texture.MIN_FILTER, osg.Texture.LINEAR_MIPMAP_LINEAR);
                    tex.setFilter(osg.Texture.MAG_FILTER, osg.Texture.LINEAR);
                }
            }
        }

        private string output_location;
        private osgDB.Archive archive;
        private bool compress_textures;
        private bool inline_textures;
        private bool fix_mipmaps;
        private uint max_tex_size;

        private const bool DEFAULT_COMPRESS_TEXTURES = false;
        private const int DEFAULT_MAX_TEX_SIZE = 0;      // 0 => no max
        private const bool DEFAULT_INLINE_TEXTURES = false;
        private const bool DEFAULT_FIX_MIPMAPS = true;
    }
}