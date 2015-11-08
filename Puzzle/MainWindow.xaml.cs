using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Puzzle
{
    public partial class MainWindow : Window
    {
        public PuzzleModel Model = new PuzzleModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = Model;
            LoadImage("pack://application:,,,/Puzzle;component/TheBurningRange.jpg");
        }

        void CommandBinding_Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int val = (int)e.Parameter;
            Model.DoAction(val, (canMove, zero, cur) =>
            {

                Debug.WriteLine($"Diff = {zero - cur}; ({cur} + 1) % {Model.MaxColumns} = {(cur + 1) % Model.MaxColumns}; curIndex - zero = {cur - zero}");

                if (canMove)
                {
                    Model.Cells[zero].Index = val;
                    Model.Cells[cur].Index = 0;
                }
            });

            if (Model.IsCorrect())
                MessageBox.Show("Congratulation, you win!", "Puzzle", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        void CommandBinding_Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Model.UseHepler)
                Model.DoAction((int)e.Parameter, (canMove, zero, cur) => e.CanExecute = canMove);
            else
                e.CanExecute = true;
        }

        void CommandBinding_Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Filter = "Image Files (*.jpg;*.bmp;*.png)|*.jpg;*.bmp;*.png";

                if (dlg.ShowDialog() == true)
                {
                    LoadImage(dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        void LoadImage(string path, UriKind uriKind = UriKind.Absolute)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, uriKind);
            bitmap.EndInit();
            Model.ImageSource = bitmap;
        }

        void CommandBinding_Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}