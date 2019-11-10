using System;
using System.Collections.Generic;

namespace CinemaScraper
{
    public class Movie
    {
        public int Id {get; set;}
        public string Title {get; set;}
        public bool Dubbed {get; set;}
        public DateTime InCinemaFrom {get; set;}
        public Cinema ShownIn {get; set;}
        public DateTime Updated {get; set;}


        public override string ToString() => $"\"{Title}\". From {InCinemaFrom.ToString("yyyy-MM-dd")}. Dubbed = {Dubbed}";
    }
}
