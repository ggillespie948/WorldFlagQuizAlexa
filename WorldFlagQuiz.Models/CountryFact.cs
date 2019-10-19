using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorldFlagQuiz.Models
{
    public class CountryFact
    {
        [Key]
        public int Id { get; set; }
        public string FactText { get; set; }
        public int CountryFlagId { get; set; }
        public virtual CountryFlag CountryFlag { get; set; }

    }
}
