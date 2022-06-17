using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Umsetzung_II
{
    public class KlickCommand : ICommand
    {
        private readonly ViewModel viewModel;

        public event EventHandler? CanExecuteChanged;

        public KlickCommand(ViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            viewModel.ClickCounter++;
        }

    }
}
