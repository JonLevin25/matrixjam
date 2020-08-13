using System;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class NumberButtonsManager
    {
        public static int BOARD_SIZE = 9;
        private static float SMALL_PADDING = 1;
        private static float LARGE_PADDING = 3;
        private static NumberButtonScript[][] _allButtons = new NumberButtonScript[BOARD_SIZE][];

        public static void GenerateBoard(NumberButtonScript numberPrefab)
        {
            var squareWidth = SquareWidth(numberPrefab);
            var originalPosition = numberPrefab.transform.localPosition;
            
            float horizontalPad = 0;
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                if (i == 3 || i == 6)
                {
                    horizontalPad += LARGE_PADDING;
                }
                _allButtons[i] = new NumberButtonScript[BOARD_SIZE];
                float verticalPad = 0;
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (j == 3 || j == 6)
                    {
                        verticalPad += LARGE_PADDING;
                    }
                    Vector3 position = originalPosition + new Vector3(i * squareWidth+horizontalPad, -(j * squareWidth+ verticalPad), 0);
                    var clone = GameObject.Instantiate(numberPrefab, numberPrefab.transform.parent);
                    clone.transform.localPosition = position;
                    clone.Index = new Vector2(i,j);
                    _allButtons[i][j] = clone;
                    clone.Clear();
                }
            }
            numberPrefab.gameObject.SetActive(false);
        }

        public static float SquareWidth(NumberButtonScript numberPrefab)
        {
            var renderer = numberPrefab.GetComponent<RectTransform>();
            var squareWidth = renderer.rect.width + SMALL_PADDING;
            return squareWidth;
        }

        public static void SetSquaresForPuttingNumberOn(Vector2[] squares)
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    _allButtons[i][j].MakeSelectable(IsInSelectableSquares(i, j, squares));
                }
            }
                
        }

        private static bool IsInSelectableSquares(int i, int j, Vector2[] squares)
        {
            foreach (var square in squares)
            {
                if (square.x == i && square.y == j)
                {
                    return true;

                }
            }

            return false;
        }

        public static void MakeAllUnselectable()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    _allButtons[i][j].MakeSelectable(false);
                }
            }
        }

        public static void SetNumberOnSquare(Vector2 index, int currentValue, PlayerSide playerColor)
        {
            var numberButtonScript = _allButtons[(int) index.x][(int) index.y];
            if (!numberButtonScript.HasValue())
            {
                numberButtonScript.SetValue(currentValue, playerColor);
            }
        }
    }
}