using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umsetzung_III
{
    // Command, der im ViewModel alles zuruecksetzt
    internal class ResetAllCommand : CommandBase
    {
        private readonly ViewModel viewModel;
        public ResetAllCommand(ViewModel viewModel)
        {
            this.viewModel = viewModel;

        }
        public override void Execute(object? parameter)
        {
            viewModel.ResetViewModel();

        }
    }
}
