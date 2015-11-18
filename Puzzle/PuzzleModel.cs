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

        #region Properties

        BitmapSource imageSource;
        public BitmapSource ImageSource
        {
            get { return imageSource; }
            set
            {
                if (value != imageSource)
                {
                    imageSource = value;
                    RaiseEvent();
                    Fill();
                }
            }
        }

        int maxColumns = 4;
        public int MaxColumns
        {
            get { return maxColumns; }
            set
            {
                if (value != maxColumns)
                {
                    maxColumns = value;
                    RaiseEvent();
                    Fill();
                }
            }
        }

        #endregion

        void Fill()
        {
            //MaxColumns = max;
            Cells.Clear();

            var random = new Random();
            var size   = MaxColumns * MaxColumns;
            var list   = Enumerable.Range(0, size).ToList();

#if DEBUG
            // enumerable fill
            foreach (var i in list)
                Cells.Add(new PuzzleUnit(i, this));
#else
            // random fill
            while (list.Count > 0)
            {
                int index = random.Next(0, list.Count - 1);
                Cells.Add(new PuzzleUnit(list[index], this));
                list.RemoveAt(index);
            }
#endif
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