using System;
namespace BookList.Interaction
{
    [Serializable]
    public class SearchingResult : DataBookInfo, IComparable    
    {                                                       
        public int count { get; private set; }              
        public bool isFromAnnotation { get; private set; }  

        public SearchingResult(string name, string author, string annotation, int ISBN, DateTime publicationDate, int count, bool isFromAnnotation)
            : base(name, author, annotation, ISBN, publicationDate)
        {
            this.count = count;
            this.isFromAnnotation = isFromAnnotation;
        }

        public SearchingResult()
        {
        }

        public int CompareTo(object o)  
        {
            SearchingResult compareObj = o as SearchingResult;
            if (compareObj != null)
                return this.count.CompareTo(compareObj.count);
            else
                throw new Exception("Sorting is not possible");
        }
    }
}