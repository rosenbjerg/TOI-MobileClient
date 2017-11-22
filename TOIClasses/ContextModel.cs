using System;
using System.Collections.Generic;
using System.Text;

namespace TOIClasses
{
    public class ContextModel : ModelBase
    {
        public ContextModel()
        {

        }
        public ContextModel(string id, string title, string description = null)
        {
            Id = id;
            Title = title;
            Description = description;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is ContextModel t && t.Id == Id;
        }

        protected bool Equals(ContextModel other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
