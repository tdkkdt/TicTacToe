using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe {
    class Program {
        static void Main(string[] args) {

        }
    }
    enum Figure : byte {
        Empty = 0,
        Nought = 1,
        Cross = 2
    }

    struct GameState {
        Figure[,] grid;

        public Figure this[int r, int c] => grid?[r, c] ?? Figure.Empty;

        public static GameState FromString(string gridAsString) {
            GameState result = new GameState();
            result.grid = new Figure[3, 3];
            for (int r = 0; r < 3; r++) {
                for (int c = 0; c < 3; c++) {
                    char gridAsStringCell = gridAsString[r * 3 + c];
                    switch (gridAsStringCell) {
                        case 'X':
                            result.grid[r, c] = Figure.Cross;
                            break;
                        case 'O':
                            result.grid[r, c] = Figure.Nought;
                            break;
                        case '.':
                            result.grid[r, c] = Figure.Empty;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(gridAsStringCell));
                    }
                }
            }
            return result;
        }
    }
}
