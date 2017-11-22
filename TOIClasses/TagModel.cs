namespace TOIClasses
{
    public class TagModel : ModelBase
    {
        public TagModel()
        {

        }
        public TagModel(string id, TagType type)
        {
            Id = id;
            Type = type;
        }
        
        public TagType Type { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Radius { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TagModel t && t.Id == Id;
        }

        protected bool Equals(TagModel other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}