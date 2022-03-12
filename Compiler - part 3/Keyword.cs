using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCompiler
{
    public class Keyword : Token
    {
        public string Name { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is Keyword)
            {
                Keyword t = (Keyword)obj;
                if (Name == t.Name)
                    return base.Equals(t);
            }
            return false;
        }
        public override string ToString()
        {
            return Name;
        }

    }
}
