using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.Models
{
    public class SnakesLaddersMap
    {
        private Dictionary<int, CellDetails> _mapDetails;
        public Dictionary<int, Vector2> _cellPositions;

        public int WinCell { get; set; }
        public int StartCell { get; set; } = 1;
        public SnakesLaddersMap()
        {
            _mapDetails = new Dictionary<int, CellDetails>();
        }

        public void AddCellDetail(int cellNo, CellDetails cellDetails)
        {
            _mapDetails.Add(cellNo, cellDetails);
        }

        public CellDetails GetCellDetails(int cellNo)
        {
            if(_mapDetails.ContainsKey(cellNo))
            {
                return _mapDetails[cellNo];
            }

            return null;
        }
    }
}
