using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.ViewModels
{
    public class RoundUpdateResults
    {
        public Round? Round { get; set; }
        public Discussion? Discussion { get; set; }
        public Book? Book { get; set; }
        public Poll? Poll { get; set; }
    }
}
