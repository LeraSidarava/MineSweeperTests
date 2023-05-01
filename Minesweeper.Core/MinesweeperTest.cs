using System;
using Minesweeper.Core.Enums;
using NUnit.Framework;

namespace Minesweeper.Core
{
    [TestFixture]
    public class MinesweeperTest
    {
        public static object[] Open_TestCases =
        {

        new object[] { new bool[,]
            {
                { false, false, false },
                { false, false, true },
                { false, false, false }
            },
            0, 0, PointState.Neighbors0 },

        new object[] { new bool[,]
            {
                { false, true, false },
                { false, false, true },
                { false, false, false }
            },
            0, 0, PointState.Neighbors1 },

        new object[] { new bool[,]
            {
                { false, true, false },
                { false, false, true },
                { false, false, false }
            },
            0, 2, PointState.Neighbors2 }
    };

        [TestCaseSource(nameof(Open_TestCases))]


        public void T1_Open_WithMineNeighbor_RevealsNumberOfMineNeighbors(
            bool[,] boolField, int x, int y, PointState expectedState)
        {
            // Precondition
            var gameProcessor = new GameProcessor(boolField);

            // Action
            gameProcessor.Open(x, y);

            // Assert
            var currentField = gameProcessor.GetCurrentField();
            Assert.AreEqual(expectedState, currentField[y, x]);
        }





        [Test]
        public void T2_Open_AllNonMineCells_SetGameStateIsWin()
        {
            // Precondition
            bool[,] boolField = {
        { true, false, false },
        { false, false, false },
        { false, false, true },
    };
            GameProcessor processor = new GameProcessor(boolField);

            // Action
            processor.Open(0, 1);
            processor.Open(0, 2);
            processor.Open(1, 0);
            processor.Open(1, 2);
            processor.Open(2, 0);
           

            // Assert: Verify that game state is 'Win'
            Assert.AreEqual(GameState.Win, processor.GameState);
        }



        [Test]
        public void T3_Open_MineCell_SetGameStateIsLose()
        {
            // Precondition
            bool[,] boolField = {
                { true, false },
                { false, false },
            };
            GameProcessor processor = new GameProcessor(boolField);

            // Action
            processor.Open(0, 0);

            // Assert

            Assert.AreEqual(GameState.Lose, processor.GameState);
        }


        [Test]
        public void T4_Open_GameStateIsUnchanged_WhenCalledOnAlreadyOpenedCell()
        {
            // Precondition
            bool[,] boolField = {
                { false, false , false},
                { false, false , false},
                { false, false , false},
            };
            GameProcessor processor = new GameProcessor(boolField);
            processor.Open(0, 1);

            // Action
            GameState oldState = processor.GameState;
            processor.Open(0, 01);

            // Assert
            Assert.AreEqual(oldState, processor.GameState);
        }





        [Test]
        public void T5_Open_MethodCannotBeCalled_WhenGameFinished()
        {
            // Precondition
            bool[,] boolField = new bool[2, 2] { { false, true }, { true, false } };
            var game = new GameProcessor(boolField);

            // Action
            game.Open(0, 0);
            game.Open(1, 1);

            // Assert

            Assert.Throws<InvalidOperationException>(() => game.Open(1, 0));
            Assert.AreEqual(GameState.Win, game.GameState);
        }

        // Tests for 

        [Test]
        public void T6_Open_MineNeighborsCountOfAllOtherCellsIsCorrect()
        {
            // Precondition
            var boolfield = new bool[,]
            {
              { true, false, true },
              { false, true, false },
              { true, false, true }
            };

            var game = new GameProcessor(boolfield);

            // Action
            game.Open(1, 1);

            // Assert
            var expectedField = new PointState[,]
            {

                  { PointState.Neighbors3, PointState.Neighbors2, PointState.Neighbors3 },
                  { PointState.Neighbors2, PointState.Mine, PointState.Neighbors2 },
                  { PointState.Neighbors3, PointState.Neighbors2, PointState.Neighbors3 }

            };


        }

        [Test]
        public void T7_Open_CellWithNoNeighboringMines_OpensAllAdjacentCells()
        {
            // Precondition
            var boolfield = new bool[,]
            {
                {false, false, false },
                { false, false, false },
                { false, false, false }
            };



            var game = new GameProcessor(boolfield);

            // Action
            game.Open(1, 1);

            // Assert
            var currentField = game.GetCurrentField();
            for (int i = 0; i < currentField.GetLength(0); i++)
            {
                for (int j = 0; j < currentField.GetLength(1); j++)
                {
                    Assert.AreEqual(PointState.Neighbors0, currentField[i, j]);
                }
            }

        }



        ////TEST fot GETfield

        [Test]
        public void T8_GetCurrentField_ShouldReturnSameDimensionsAsField()
        {
            // Precondition
            bool[,] boolField = new bool[3, 4]
            {
                { false, false, false, false },
                { false, true, false, true },
                { false, false, false, true }
            };


            var game = new GameProcessor(boolField);

            // Action
            var currentField = game.GetCurrentField();

            // Assert
            Assert.AreEqual(boolField.GetLength(0), currentField.GetLength(0));
            Assert.AreEqual(boolField.GetLength(1), currentField.GetLength(1));
        }

        [Test]
        public void T9_GetCurrentField_ShouldReturnCorrectValues_WhenAllCellsAreClosed()
        {
            // Precondition
            bool[,] boolField = new bool[3, 3]
            {
               { false, false, false },
               { false, false, false },
               { false, false, false }
            };
            var game = new GameProcessor(boolField);

            PointState[,] expectedField = new PointState[3, 3]
            {
                { PointState.Close, PointState.Close, PointState.Close },
                { PointState.Close, PointState.Close, PointState.Close },
                { PointState.Close, PointState.Close, PointState.Close }
            };


            // Action
            PointState[,] actualField = game.GetCurrentField();

            // Assert
            Assert.AreEqual(expectedField, actualField);
        }

        [Test]
        public void T10_GetCurrentField_ShouldReturnCorrectValues_WhenSomeMinesAndNonMinesAreOpened()
        {
            // Precondition
            bool[,] boolField = new bool[3, 3]
            {
               { false, true, false },
               { true, false, false },
               { false, false, true }
            };
            var game = new GameProcessor(boolField);


            // Act
            var currentField = game.GetCurrentField();

            game.Open(1, 1);
            game.Open(0, 2);
            game.Open(2, 2);

            

            CollectionAssert.AreEqual(
             new PointState[,]
              {
                 { PointState.Close, PointState.Close, PointState.  Neighbors0 },
                 { PointState.Close, PointState.Neighbors2, PointState.Close },
                 { PointState.Close, PointState.Close, PointState.Neighbors1 }
              },
             currentField);
        }



       [Test]
        public void T11_GetCurrentField_GameFinished_ReturnsCorrectPointStateValues()
        {
            // Precondition
            bool[,] boolField = new bool[3, 3]
            {
                    { true, false, false },
                    { false, true, false },
                    { false, false, true }
            };
            var game = new GameProcessor(boolField);

            // Action
            game.Open(0, 0);    
            PointState[,] currentField = game.GetCurrentField();

            // Assert

            CollectionAssert.AreEqual(
                new PointState[,]
                {
                    {PointState.Mine,PointState.Neighbors0,PointState.Neighbors2, },
                    {PointState.Neighbors2,PointState.Mine,PointState.Neighbors2, },
                    { PointState.Neighbors2,PointState.Neighbors2,PointState.Mine,}
                },
                currentField);
            
        }


    }
}






