using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Puzzle
{
    public class PuzzleUnit : INotifyPropertyChanged
    {
        PuzzleModel puzzleModel;

        public PuzzleUnit(int index, PuzzleModel puzzle)
        {
            puzzleModel = puzzle;
            Index = index;
            Calc();
        }

        void Calc()
        {
            var xsize = puzzleModel.Image.PixelWidth  / puzzleModel.MaxColumns;
            var ysize = puzzleModel.Image.PixelHeight / puzzleModel.MaxColumns;

            var col = Index % puzzleModel.MaxColumns;
            var row = Index / puzzleModel.MaxColumns;

            var x = xsize * col;
            var y = ysize * row;

            SourceRect = new Int32Rect(x, y, xsize, ysize);
        }

        #region Propertis

        int index;
        public int Index
        {
            get { return index; }
            set
            {
                if (value != index)
                {
                    index = value;
                    RaiseEvent();
                    Calc();
                }
            }
        }

        Int32Rect sourceRect;
        public Int32Rect SourceRect
        {
            get { return sourceRect; }
            set
            {
                if (value != sourceRect)
                {
                    sourceRect = value;
                    RaiseEvent();
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseEvent([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
