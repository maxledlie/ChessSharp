using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Sharp
{
    class MainPresenter
    {
        private IMainView _view;
        private IMainModel _model;

        public MainPresenter(IMainView view, IMainModel model)
        {
            _view = view;
            _model = model;
            HookViewEvents();
        }

        private void HookViewEvents() 
        {
            _view.StartButtonClicked += StartButtonClickedHandler;
        }

        private void StartButtonClickedHandler(object sender, EventArgs args)
        {
            new GamePresenter(_view.SelectedSettings);
        }
    }
}
