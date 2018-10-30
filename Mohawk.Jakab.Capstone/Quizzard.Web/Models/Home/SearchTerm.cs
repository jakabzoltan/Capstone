namespace Quizzard.Web.Models.Home
{
    public class SearchTerm
    {
        public SearchTerm()
        {
            
        }

        public SearchTerm(string term)
        {
            Term = term;
        }
        public string Term { get; set; }
    }
}