using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongAI.Models
{
    class DefEvalResult
    {
        public double Risk { get; set; }
        public double RiskForOthers { get; set; }
        public double Bonus { get; set; }
        public EvalResult AtkEvalResult { get; set; }

        public override string ToString()
        {
            return string.Format("Risk: {0:0.##}{1}{2}",
                Risk,
                Risk == 0 ? ", For others: " + RiskForOthers.ToString("0.##") : "",
                Bonus != 0 ? ", For atk: -" + Bonus.ToString("0.##") : "");
        }
    }
}
