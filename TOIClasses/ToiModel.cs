using System.Collections.Generic;

namespace TOIClasses
{
    public enum ToiInformationType
    {
        Image, Audio, Video, Website, Text
    }
    public class ToiModel : ModelBase
    {
        public string Image { get; set; }
        public string Url { get; set; }
        public ToiInformationType InformationType { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Contexts { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ToiModel t && t.Id == Id;
        }

        protected bool Equals(ToiModel other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}