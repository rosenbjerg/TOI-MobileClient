using System;
using System.Collections.Generic;
using System.Text;

namespace TOIClasses
{
    public class StaticFile : ModelBase
    {
        public string Filetype { get; set; }

        public DateTime CreatedOn { get; set; }

        public StaticFile()
        {
            CreatedOn = DateTime.Now;
        }

        public string GetFilename()
        {
            return Id + "." + Filetype;
        }
    }
}
