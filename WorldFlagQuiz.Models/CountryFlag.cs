using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorldFlagQuiz.Models
{
    public class CountryFlag
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Alpha2Code { get; set; }

        public string Alpha3Code { get; set; }

        public string NumericCode { get; set; }

        public string SmallSourceUrl { get; set; }

        public string LargeSourceUrl { get; set; }

        public Difficulty Difficulty { get; set; }

        public virtual ICollection<CountryFact> CountryFacts { get; set; }
    }

    public enum Difficulty
    {
        VeryEasy = 0,
        Easy = 1,
        Medium = 2,
        Hard = 3,
        VeryHard = 4
    }

}
