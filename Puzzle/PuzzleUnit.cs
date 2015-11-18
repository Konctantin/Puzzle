using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Puzzle
{
    public class PuzzleUnit : INotifyPropertyChanged
    {
        public PuzzleModel Parent { get; private set; }

        public CroppedBitmap ImageSource { get; private set; }

        public PuzzleUnit(int index, PuzzleModel puzzle)
        {
            Parent = puzzle;
            Index = index;
            Calc();
        }

        void Calc()
        {
            var xsize = Parent.ImageSource.PixelWidth  / Parent.MaxColumns;
            var ysize = Parent.ImageSource.PixelHeight / Parent.MaxColumns;

            var col = Index % Parent.MaxColumns;
            var row = Index / Parent.MaxColumns;

            var x = xsize * col;
            var y = ysize * row;

            var rect = new Int32Rect(x, y, xsize, ysize);
            ImageSource = new CroppedBitmap(Parent.ImageSource, rect);
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
                    RaiseEvent("ImageSource");
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
