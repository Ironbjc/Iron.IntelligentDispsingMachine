using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.Models
{
    public enum Direction
    {
        Left=1,
        Right=2
    }
    public enum RowPosition
    {
        First=1,
        Middle=2,
        Third=3
    }
    public class MotorPositon
    {
        public Direction direction { get; set; }
        public RowPosition rowPosition { get; set; }
    }
}
