using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookList.Interaction
{
    [Serializable]
    public class DataBookInfo
    {
        public string name { get; private set; }
        public string author { get; private set; }
        public string annotation { get; private set; }
        public int ISBN { get; private set; }
        public DateTime publicationDate { get; private set; }

        public DataBookInfo() { }        

        public DataBookInfo(string name, string author, string annotation,
                            int ISBN, DateTime publicationDate)
        {
            this.name = name;
            this.author = author;
            this.annotation = annotation;
            this.ISBN = ISBN;
            this.publicationDate = publicationDate;
        }
    }
}
