using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Field : IDisposable
    {
        private int mWidth;
        private int mHeight;
        private Vector3 mCenter;
        
        private Cell[,] mCells;
        
        public Vector3 Center => mCenter;
        public Cell[,] Cells => mCells;
        public int Width => mWidth;
        public int Height => mHeight;

        private float mScale;
        public Vector3 MinXYCellCenter()
        {
            Vector3 HalfWidthLeft = Vector3.left *  ((float) (mWidth)  * 0.5f - 0.5f)* mScale;
            Vector3 HalfHeightDown = Vector3.down * ((float) (mHeight) * 0.5f - 0.5f) * mScale;

            return mCenter + HalfWidthLeft + HalfHeightDown;
        }
        
        public Vector3 StackBottom()
        {
            Vector3 CellLeft = Vector3.left *  (1f) * mScale;
            return MinXYCellCenter() + CellLeft * 2f;
        }
        
        public void DropToStack(float stackHeight, float stackWidth)
        {
            for (int i =0; i < mWidth; i++)
            {
                for (int j =0; j < mHeight;j++)
                {
                    mCells[i, j].DropToStack(StackBottom(), stackHeight, stackWidth);
                }   
            }
        }
        
        public Field(int w, int h, Vector3 center, GameObject cellPrefab, float scale, Canvas canvas)
        {
            mWidth = w;
            mHeight = h;
            mCenter = center;
            mScale = scale;
            
            Cell[,] cells = new Cell[mWidth, mHeight];
            
            var allNums = Enumerable.Range(Constants.MIN_NUM_VALUE, Constants.MAX_NUM_VALUE - Constants.MIN_NUM_VALUE);
            var rnd = new System.Random();
            var randomized = allNums.OrderBy(item => rnd.Next()).ToArray();
            int randIdx = 0;
            for (int i =0; i < mWidth; i++)
            {
                for (int j =0; j < mHeight;j++)
                {
                    var cellNum = randomized[randIdx];
                    var color = C(cellNum,Constants.MIN_NUM_VALUE,Constants.MAX_NUM_VALUE);
                    var zOffset = mWidth + (float) (i * mWidth + j * mHeight);
                    var cell = new Cell(i,j,cellNum,color, cellPrefab, MinXYCellCenter(), canvas, scale, zOffset * scale / 100000f);
                    cells[i, j] = cell;
                    randIdx++;
                }   
            }

            mCells = cells;
        }
        
        public static Color C(int numRGB, int min, int max)
        {
            var partH = Mathf.InverseLerp(min, max, numRGB);  // range 0 100 tone to 0 1 hue value
            //color coding
            var digitcolortonergb = Color.HSVToRGB(partH, 1f, 1f);             // to rgb
            return digitcolortonergb;
        }
        
        public string GetCheckResult()
        {
            var count = 0;
            foreach (var mCell in mCells)
            {
                if (mCell.Check()) count++;
            }

            return $"{count}/{Width * Height}";
        }
        
        public void RenderBorder(Material lineMaterial)
        {
            foreach (var mCell in mCells)
            {
                mCell.RenderBorder(lineMaterial);
            }
        }
        
        public void Dispose()
        {
            for (int i =0; i < mWidth; i++)
            {
                for (int j =0; j < mHeight;j++)
                {
                    mCells[i, j]?.Dispose();
                }   
            }
        }
    }    
}

