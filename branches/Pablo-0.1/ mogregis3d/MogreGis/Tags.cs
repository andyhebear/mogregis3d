using System;
using System.Collections.Generic;


namespace MogreGis
{
    public class TagList : List<string> { }
    public class TagSet : HashSet<string> { }

    /* (internal)
     * Helper class that implements tagging.
     */
    public class TagHelper
    {
        public void addTag(string _tag)
        {
            tags.Add(normalize(_tag));
        }

        public void removeTag(string _tag)
        {
            tags.Remove(normalize(_tag));
        }

        public bool containsTag(string _tag)
        {
            return tags.Contains(normalize(_tag));
        }

        public bool containsTags(TagList _tags)
        {
            foreach (string i in _tags)
            {
                if (!tags.Contains(normalize(i)))
                    return false;
            }
            return true;
        }

        public TagSet getTags() { return tags; }


        private TagSet tags = new TagSet();
        private string normalize(string input)
        {
            return input.ToLowerInvariant();
        }
    }

    public interface IObjectWithTags
    {
        void addTag(string tag);
        void removeTag(string tag);
        bool containsTag(string tag);
        bool containsTags(TagList tags);
        TagSet getTags();
    }

    public class ObjectWithTags : IObjectWithTags
    {

        public virtual void addTag(string tag) { helper.addTag(tag); }
        public virtual void removeTag(string tag) { helper.removeTag(tag); }
        public virtual bool containsTag(string tag) { return helper.containsTag(tag); }
        public virtual bool containsTags(TagList tags) { return helper.containsTags(tags); }
        public virtual TagSet getTags() { return helper.getTags(); }

        private TagHelper helper = new TagHelper();
    }
}
