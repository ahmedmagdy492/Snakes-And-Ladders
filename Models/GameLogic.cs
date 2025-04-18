using Microsoft.Xna.Framework;
using SnakeAndLadders.Services;
using System;
using System.Collections.Generic;

namespace SnakeAndLadders.Models
{
    public enum GameState
    {
        Playing,
        Paused,
        Ended
    }
    
    public enum GamePlayMode
    {
        AganistComputer,
        AganistPlayer
    }

    public class GameLogic
    {
        private readonly List<SnakesLaddersMap> maps;
        public readonly SnakesLaddersMap _currentMap;
        private readonly List<Player> _players;
        private int _currentPlayerNo = 0;
        private Player _currentPlayingPlayer;
        private readonly GamePlayMode _gamePlayMode;
        private readonly DiceRollerService _randNumGenService;

        public event Action<Player> OnWining;

        private Player DeterminePlayerTurn()
        {
            Random rand = new Random();
            _currentPlayerNo = rand.Next(0, _players.Count);
            return _players[_currentPlayerNo];
        }

        private void ResetPlayersPositions()
        { 
            _players[0].Position = _currentMap._cellPositions[1];
            _players[1].Position = _currentMap._cellPositions[1];
            _players[1].Position = new Vector2(_players[1].Position.X + 10, _players[1].Position.Y);
        }

        private int DetermineNextPlayerCell(int newPlayerCellNo)
        {
            if (newPlayerCellNo >= _currentMap.WinCell)
            {
                return _currentMap.WinCell;
            }

            var cellDetails = _currentMap.GetCellDetails(newPlayerCellNo);
            if (cellDetails != null)
            {
                if (cellDetails.HasSnake)
                {
                    return cellDetails.SnakeEndCellNo;
                }
                if (cellDetails.HasLadder)
                {
                    return cellDetails.LadderEndCellNo;
                }
            }

            return newPlayerCellNo;
        }

        public GameLogic(List<Player> players, GamePlayMode gamePlayMode)
        {
            if (players == null || players.Count == 0)
                throw new ArgumentException("Invalid players argument");

            _randNumGenService = new DiceRollerService();
            maps = new List<SnakesLaddersMap>();
            _currentMap = new SnakesLaddersMap
            {
                StartCell = 1,
                WinCell = 100,
            };
            #region filling out a default map 
            _currentMap.AddCellDetail(5, new CellDetails
            {
                CellNo = 5,
                HasLadder = true,
                LadderEndCellNo = 58
            });
            _currentMap.AddCellDetail(14, new CellDetails
            {
                CellNo = 14,
                HasLadder = true,
                LadderEndCellNo = 49
            });
            _currentMap.AddCellDetail(38, new CellDetails
            {
                CellNo = 38,
                HasSnake = true,
                SnakeEndCellNo = 20
            });
            _currentMap.AddCellDetail(42, new CellDetails
            {
                CellNo = 42,
                HasLadder = true,
                LadderEndCellNo = 60
            });
            _currentMap.AddCellDetail(45, new CellDetails
            {
                CellNo = 45,
                HasSnake = true,
                SnakeEndCellNo = 7
            });
            _currentMap.AddCellDetail(51, new CellDetails
            {
                CellNo = 51,
                HasSnake = true,
                SnakeEndCellNo = 10
            });
            _currentMap.AddCellDetail(53, new CellDetails
            {
                CellNo = 53,
                HasLadder = true,
                LadderEndCellNo = 72
            });
            _currentMap.AddCellDetail(65, new CellDetails
            {
                CellNo = 65,
                HasSnake = true,
                SnakeEndCellNo = 54
            });
            _currentMap.AddCellDetail(122, new CellDetails
            {
                CellNo = 122,
                HasSnake = true,
                SnakeEndCellNo = 119
            });
            _currentMap.AddCellDetail(75, new CellDetails
            {
                CellNo = 75,
                HasLadder = true,
                LadderEndCellNo = 94
            });
            _currentMap.AddCellDetail(91, new CellDetails
            {
                CellNo = 91,
                HasSnake = true,
                SnakeEndCellNo = 73
            });
            _currentMap.AddCellDetail(97, new CellDetails
            {
                CellNo = 97,
                HasSnake = true,
                SnakeEndCellNo = 61
            });
            #endregion
            _currentMap._cellPositions = new Dictionary<int, Vector2>()
            {
                [1] = new Vector2(30, 570),
                [2] = new Vector2(90, 570),
                [3] = new Vector2(150, 570),
                [4] = new Vector2(210, 570),
                [5] = new Vector2(270, 570),
                [6] = new Vector2(330, 570),
                [7] = new Vector2(390, 570),
                [8] = new Vector2(450, 570),
                [9] = new Vector2(510, 570),
                [10] = new Vector2(570, 570),
                [11] = new Vector2(570, 510),
                [12] = new Vector2(510, 510),
                [13] = new Vector2(450, 510),
                [14] = new Vector2(390, 510),
                [15] = new Vector2(330, 510),
                [16] = new Vector2(270, 510),
                [17] = new Vector2(210, 510),
                [18] = new Vector2(150, 510),
                [19] = new Vector2(90, 510),
                [20] = new Vector2(30, 510),
                [21] = new Vector2(30, 450),
                [22] = new Vector2(90, 450),
                [23] = new Vector2(150, 450),
                [24] = new Vector2(210, 450),
                [25] = new Vector2(270, 450),
                [26] = new Vector2(330, 450),
                [27] = new Vector2(390, 450),
                [28] = new Vector2(450, 450),
                [29] = new Vector2(510, 450),
                [30] = new Vector2(570, 450),
                [31] = new Vector2(570, 390),
                [32] = new Vector2(510, 390),
                [33] = new Vector2(450, 390),
                [34] = new Vector2(390, 390),
                [35] = new Vector2(330, 390),
                [36] = new Vector2(270, 390),
                [37] = new Vector2(210, 390),
                [38] = new Vector2(150, 390),
                [39] = new Vector2(90, 390),
                [40] = new Vector2(30, 390),
                [41] = new Vector2(30, 330),
                [42] = new Vector2(90, 330),
                [43] = new Vector2(150, 330),
                [44] = new Vector2(210, 330),
                [45] = new Vector2(270, 330),
                [46] = new Vector2(330, 330),
                [47] = new Vector2(390, 330),
                [48] = new Vector2(450, 330),
                [49] = new Vector2(510, 330),
                [50] = new Vector2(570, 330),
                [51] = new Vector2(570, 270),
                [52] = new Vector2(510, 270),
                [53] = new Vector2(450, 270),
                [54] = new Vector2(390, 270),
                [55] = new Vector2(330, 270),
                [56] = new Vector2(270, 270),
                [57] = new Vector2(210, 270),
                [58] = new Vector2(150, 270),
                [59] = new Vector2(90, 270),
                [60] = new Vector2(30, 270),
                [61] = new Vector2(30, 210),
                [62] = new Vector2(90, 210),
                [63] = new Vector2(150, 210),
                [64] = new Vector2(210, 210),
                [65] = new Vector2(270, 210),
                [66] = new Vector2(330, 210),
                [67] = new Vector2(390, 210),
                [68] = new Vector2(450, 210),
                [69] = new Vector2(510, 210),
                [70] = new Vector2(570, 210),
                [71] = new Vector2(570, 150),
                [72] = new Vector2(510, 150),
                [73] = new Vector2(450, 150),
                [74] = new Vector2(390, 150),
                [75] = new Vector2(330, 150),
                [76] = new Vector2(270, 150),
                [77] = new Vector2(210, 150),
                [78] = new Vector2(150, 150),
                [79] = new Vector2(90, 150),
                [80] = new Vector2(30, 150),
                [81] = new Vector2(30, 90),
                [82] = new Vector2(90, 90),
                [83] = new Vector2(150, 90),
                [84] = new Vector2(210, 90),
                [85] = new Vector2(270, 90),
                [86] = new Vector2(330, 90),
                [87] = new Vector2(390, 90),
                [88] = new Vector2(450, 90),
                [89] = new Vector2(510, 90),
                [90] = new Vector2(570, 90),
                [91] = new Vector2(570, 30),
                [92] = new Vector2(510, 30),
                [93] = new Vector2(450, 30),
                [94] = new Vector2(390, 30),
                [95] = new Vector2(330, 30),
                [96] = new Vector2(270, 30),
                [97] = new Vector2(210, 30),
                [98] = new Vector2(150, 30),
                [99] = new Vector2(90, 30),
                [100] = new Vector2(30, 30),
            };

            maps.Add(_currentMap);
            _players = players;
            _gamePlayMode = gamePlayMode;
            if(_gamePlayMode == GamePlayMode.AganistComputer)
            {
                _currentPlayingPlayer = DeterminePlayerTurn();
            }
            else
            {
                // server player will always play first
                _currentPlayingPlayer = _players[0];
            }
            ResetPlayersPositions();
        }

        public int GenRandNum()
        {
            return _randNumGenService.GenRandNum();
        }

        public void ResetGame()
        {
            ResetPlayersPositions();
            foreach(var player in _players)
            {
                player.CurrentCellNo = 1;
                player.Position = _currentMap._cellPositions[player.CurrentCellNo];
            }
            _currentPlayingPlayer = DeterminePlayerTurn();
        }

        public Player GetCurrentPlayingPlayer()
        {
            return _currentPlayingPlayer;
        }

        public List<Player> GetPlayers()
        {
            return _players;
        }

        public void MoveCurrentPlayingPlayer(int diceValue)
        {
            int newPlayerCellNo = _currentPlayingPlayer.CurrentCellNo + diceValue;
            _currentPlayingPlayer.MovingCellNo = _currentPlayingPlayer.CurrentCellNo;
            _currentPlayingPlayer.CurrentCellNo = DetermineNextPlayerCell(newPlayerCellNo);
            if(_currentPlayingPlayer.CurrentCellNo >= _currentMap.WinCell)
            {
                if (OnWining != null)
                {
                    OnWining(_currentPlayingPlayer);
                }
            }

            _currentPlayingPlayer.Position = _currentMap._cellPositions[_currentPlayingPlayer.CurrentCellNo];
            var otherPlayer = _currentPlayingPlayer == _players[0] ? _players[1] : _players[0];
            if (_currentPlayingPlayer.CurrentCellNo == otherPlayer.CurrentCellNo)
            {
                _currentPlayingPlayer.Position = new Vector2(_currentPlayingPlayer.Position.X + 10, _currentPlayingPlayer.Position.Y);
            }
            else
            {
                _currentPlayingPlayer.Position = new Vector2(_currentPlayingPlayer.Position.X, _currentPlayingPlayer.Position.Y);
            }
        }

        public void ChangePlayerTurn()
        {
            _currentPlayerNo++;
            if (_currentPlayerNo >= _players.Count)
            {
                _currentPlayerNo = 0;
            }

            _currentPlayingPlayer = _players[_currentPlayerNo];
        }
    }
}
