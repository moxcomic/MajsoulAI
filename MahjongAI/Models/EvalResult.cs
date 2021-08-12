using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongAI.Models
{
    class EvalResult
    {
        public bool Furiten { get; set; }
        public int Distance { get; set; }
        public int NormalDistance { get; set; }
        public int VisibleFuuroCount { get; set; }
        public int DiscardedDoraCount { get; set; }
        public int DoraCount { get; set; }
        public int DoraInFuuroCount { get; set; }
        public int KanCount { get; set; }
        public List<double> E_PromotionCount { get; set; }
        public int E_NormalPromitionCount { get; set; }
        public int E_Point { get; set; }
        public bool ProbablyChiitoitsu { get; set; } = false;
        public int OptimizationCount { get; set; } = -1;

        public override string ToString()
        {
            return ToString(int.MaxValue);
        }
        
        public string ToString(int currentDistance)
        {
            return string.Format("E_PromotionCount: [{0}]{1}{5}{2}{3}{4}",
                E_PromotionCount.ToString(", ", c => c.ToString("F0")),
                E_NormalPromitionCount != E_PromotionCount[0] ? "(" + E_NormalPromitionCount + ")" : "",
                E_Point > 0 ? ", E_Point: " + E_Point : "",
                Distance > currentDistance ? ", Distance++" : "",
                ProbablyChiitoitsu ? ", Probably Chiitoitsu" : "",
                OptimizationCount != -1 ? ", OptimizationCount: " + OptimizationCount : "");
        }
    }
}
