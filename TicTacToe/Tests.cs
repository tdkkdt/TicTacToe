using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TicTacToe {
    [TestFixture]
    class Tests {

        GameManager gameManager;

        [SetUp]
        public void Setup() {
            gameManager = new GameManager();
            gameManager.CalculateAllPositions();
        }
        [Test]
        public void SimpleTestCrossesWins() {
            GameState gameState = GameState.FromString(
                "X.O" +
                ".O." +
                "X.X"
            );
            Assert.AreEqual(Figure.Cross, gameManager.WhoWins(gameState));
        }

        [Test]
        public void SimpleTestNoughtsWins() {
            GameState gameState = GameState.FromString(
                "O.O" +
                ".XX" +
                "..X"
            );
            Assert.AreEqual(Figure.Nought, gameManager.WhoWins(gameState));
        }
        [Test]
        public void SimpleTestNobodyWins() {
            GameState gameState = GameState.FromString(
                "OXO" +
                "O.X" +
                "XOX"
            );
            Assert.AreEqual(Figure.Empty, gameManager.WhoWins(gameState));
        }

        [Test]
        public void FullTest() {
            GameState gameState = GameState.FromString(
                "..." +
                "..." +
                "..."
            );
            Assert.AreEqual(Figure.Empty, gameManager.WhoWins(gameState));
        }
    }
}
