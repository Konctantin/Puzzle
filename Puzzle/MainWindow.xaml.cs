using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Puzzle
{
    public partial class MainWindow : Window
    {
        public PuzzleModel Model = new PuzzleModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = Model;
        }

        void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int val = (int)e.Parameter;
            Model.DoAction(val, (canMove, zero, cur) => {

                Debug.WriteLine($"Diff = {zero - cur}; ({cur} + 1) % {Model.MaxColumns} = {(cur + 1) % Model.MaxColumns}; curIndex - zero = {cur - zero}");

                if (canMove) {
                    Model.Cells[zero] = val;
                    Model.Cells[cur] = 0;
                }
            });

            if (Model.IsCorrect())
                MessageBox.Show("Congratulation, you win!", "Puzzle", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Model.UseHepler)
                Model.DoAction((int)e.Parameter, (canMove, zero, cur) => e.CanExecute = canMove);
            else
                e.CanExecute = true;
        }
    }
}
