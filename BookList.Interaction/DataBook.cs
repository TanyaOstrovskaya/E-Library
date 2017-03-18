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
        public string name { get; set; }
        public string author { get; set; }
        public string annotation { get; set; }
        public int ISBN { get; set; }
        public DateTime publicationDate { get; set; }

        public DataBookInfo()
        {
        }        
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
