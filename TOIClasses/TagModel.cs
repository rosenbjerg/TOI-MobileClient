namespace TOIClasses
{
    public class TagModel : LocationModel
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

        public double Latitude
        {
            get => LocationCenter.Latitude;
            set => LocationCenter.Latitude = value;
        }

        public double Longitude
        {
            get => LocationCenter.Longitude;
            set => LocationCenter.Longitude = value;
        }

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