using Obskurnee.Models;

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
