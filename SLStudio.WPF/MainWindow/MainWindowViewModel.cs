using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using SLStudio.Models;
using System.Windows.Input;

namespace SLStudio.WPF
{
    public class MainWindowViewModel: ViewModel
    {
        public MainWindowViewModel()
        {

        }

        public ICommand CreateNewSolutionCommand
        {
            get
            {
                return new RelayCommand(() => CreateNewSolution());
            }
        }

        private void CreateNewSolution()
        {
            CreateNewSolutionView createSolutionView = new CreateNewSolutionView();
            createSolutionView.ShowDialog();
        }

        private SolutionModel solutionModel;
        public SolutionModel SolutionModel
        {
            get { return solutionModel; }
            set
            {
                solutionModel = value;
                RaisePropertyChanged("SolutionModel");
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        private string statusText;
        public string StatusText
        {
            get { return statusText; }
            set
            {
                statusText = value;
                RaisePropertyChanged("StatusText");
            }
        }
    }
}
