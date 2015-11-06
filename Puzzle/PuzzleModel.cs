using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace Puzzle
{
    public class PuzzleModel : INotifyPropertyChanged
    {
        public IEnumerable<int> Sizes { get; private set; } = Enumerable.Range(4, 7);

        public ObservableCollection<PuzzleUnit> Cells { get; private set; } = new ObservableCollection<PuzzleUnit>();

        public PuzzleModel()
        {
            Fill();
        }

        #region Properties

        public CroppedBitmap Image
        {
            get; set;
        }

        double height;
        public double Height
        {
            get { return height; }
            private set
            {
                if (Math.Abs(value - height) > double.Epsilon)
                {
                    height = value;
                    RaiseEvent();
                }
            }
        }

        double wight;
        public double Width
        {
            get { return wight; }
            private set
            {
                if (Math.Abs(value - wight) > double.Epsilon)
                {
                    wight = value;
                    RaiseEvent();
                }
            }
        }

        int maxColumns;
        public int MaxColumns
        {
            get { return maxColumns; }
            set
            {
                if (value != maxColumns)
                {
                    maxColumns = value;
                    RaiseEvent();
                    Fill(value);
                }
            }
        }

        bool useHelper;
        public bool UseHepler
        {
            get { return useHelper; }
            set {
                if (value != useHelper)
                {
                    useHelper = value;
                    RaiseEvent();
                }
            }
        }

        #endregion

        public void LoadImage()
        {
        }

        void Fill(int max = 4)
        {
            MaxColumns = max;
            Width  = 96d * max;
            Height = 96d * max;

            Cells.Clear();

            Image = (CroppedBitmap)App.Current.FindResource("TheBurningRange");

            var random = new Random();
            var size   = MaxColumns * MaxColumns;
            var list   = Enumerable.Range(0, size).ToList();

            foreach (var i in list) Cells.Add(new PuzzleUnit(i, this));

            //while (list.Count > 0)
            //{
            //    int index = random.Next(0, list.Count - 1);
            //    Cells.Add(new PuzzleUnit(list[index], this));
            //    list.RemoveAt(index);
            //}
        }

        public bool IsCorrect()
        {
            for (int i = 0; i < Cells.Count; ++i)
                if (Cells[i].Index != i)
                    return false;
            return true;
        }

        public void DoAction(int current, Action<bool, int, int> predicate)
        {
            var zeroIndex = 0;
            var curIndex  = 0;
            var canMove   = false;

            for (int i = 0; i < Cells.Count; ++i)
            {
                if (Cells[i].Index == 0)
                    zeroIndex = i;

                if (Cells[i].Index == current)
                    curIndex = i;
            }

            var diff   = curIndex - zeroIndex;
            var column = (curIndex + 1) % MaxColumns;

            if      (diff == MaxColumns)  canMove = true;
            else if (diff == -MaxColumns) canMove = true;
            else if (diff == 1)           canMove = column != 1;
            else if (diff == -1)          canMove = column != 0;

            predicate(canMove, zeroIndex, curIndex);
        }

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