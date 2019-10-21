using System;
using System.Collections.Generic;

namespace TicTacToe {
    class GameManager {
        Dictionary<int, Figure> states;

        const int EmptyMask = 0;
        const int NoughtMask = 0b010101010101010101;
        const int CrossMask = 0b101010101010101010;

        static readonly int[,] PositionMasks = {{0b11, 0b11 << 2, 0b11 << 4}, {0b11 << 6, 0b11 << 8, 0b11 << 10}, {0b11 << 12, 0b11 << 14, 0b11 << 16}};

        static readonly int[] WinnerMasks = {
            PositionMasks[0, 0] | PositionMasks[0, 1] | PositionMasks[0, 2],
            PositionMasks[1, 0] | PositionMasks[1, 1] | PositionMasks[1, 2],
            PositionMasks[2, 0] | PositionMasks[2, 1] | PositionMasks[2, 2],

            PositionMasks[0, 0] | PositionMasks[1, 0] | PositionMasks[2, 0],
            PositionMasks[0, 1] | PositionMasks[1, 1] | PositionMasks[2, 1],
            PositionMasks[0, 2] | PositionMasks[1, 2] | PositionMasks[2, 2],

            PositionMasks[0, 0] | PositionMasks[1, 1] | PositionMasks[2, 2],
            PositionMasks[0, 2] | PositionMasks[1, 1] | PositionMasks[2, 0],
        };
        
        public Figure WhoWins(GameState gameState) {
            if (states == null) {
                CalculateAllPositions();
            }
            int state = CalculatePackedState(gameState);
            if (!states.TryGetValue(state, out Figure result)) {
                throw new InvalidOperationException("State is not valid");
            }
            return result;
        }

        static int FigureToInt(Figure figure) {
            switch (figure) {
                case Figure.Empty:
                    return EmptyMask;
                case Figure.Nought:
                    return NoughtMask;
                case Figure.Cross:
                    return CrossMask;
                default:
                    throw new ArgumentOutOfRangeException(nameof(figure), figure, null);
            }
        }

        static int CalculatePackedState(GameState gameState) {
            int result = 0;
            for (int r = 0; r < 3; r++) {
                for (int c = 0; c < 3; c++) {
                    result |= FigureToInt(gameState[r, c]) & PositionMasks[r, c];
                }
            }
            return result;
        }

        public void CalculateAllPositions() {
            states = new Dictionary<int, Figure>();
            var winner = CalculateChildren(0, Figure.Cross);
            states.Add(0, winner);
        }

        Figure CalculateChildren(int parentState, Figure currentFigure) {
            Figure result = MaybeIsAlreadyWinner(parentState);
            if (result != Figure.Empty) {
                return result;
            }
            Figure nextFigure = currentFigure == Figure.Cross ? Figure.Nought : Figure.Cross;
            int currentFigurePacked = FigureToInt(currentFigure);
            bool wasEmpty = false;
            int proceseedCells = 0;
            for (int r = 0; r < 3; r++) {
                for (int c = 0; c < 3; c++) {
                    if ((parentState & PositionMasks[r, c]) != 0) {
                        continue;
                    }
                    proceseedCells++;
                    int newState = parentState | (PositionMasks[r, c] & currentFigurePacked);
                    if (!states.TryGetValue(newState, out Figure winnerOnChild)) {
                        winnerOnChild = CalculateChildren(newState, nextFigure);
                        states.Add(newState, winnerOnChild);
                    }
                    if (winnerOnChild == currentFigure) {
                        result = currentFigure;
                    }
                    else if (winnerOnChild == Figure.Empty) {
                        wasEmpty = true;
                    }
                }
            }
            if (result == Figure.Empty && !wasEmpty && proceseedCells > 0) {
                result = nextFigure;
            }
            return result;
        }

        static Figure MaybeIsAlreadyWinner(int state) {
            foreach (int winnerMask in WinnerMasks) {
                int winnerMaskOnState = winnerMask & state;
                if (winnerMaskOnState == (NoughtMask & winnerMask))
                    return Figure.Nought;
                if (winnerMaskOnState == (CrossMask & winnerMask))
                    return Figure.Cross;
            }
            return Figure.Empty;
        }

        static string StateToString(int state) {
            char[] result = new char[3 * 4];
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    int figure = state & PositionMasks[i, j];
                    result[i * 4 + j] = figure == (CrossMask & PositionMasks[i, j]) ? 'X' :
                        figure == (NoughtMask & PositionMasks[i, j]) ? 'O' : '.';
                }
                result[i * 4 + 3] = '\\';
            }
            return new string(result);
        }
    }
}