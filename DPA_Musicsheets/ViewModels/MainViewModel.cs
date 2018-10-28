using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Managers.Openers;
using DPA_Musicsheets.States;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using PSAMWPFControlLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DPA_Musicsheets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
		// Handling keys is a functionality that should always be available.
		private KeyHandler keyHandler = new KeyHandler();

		private string _fileName;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        /// <summary>
        /// The current state can be used to display some text.
        /// "Rendering..." is a text that will be displayed for example.
        /// </summary>
        private string _currentState;
        public string CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; RaisePropertyChanged(() => CurrentState); }
        }

        private BaseEditorState _state;
        public BaseEditorState State
        {
            get { return _state; }
            set { _state = value; RaisePropertyChanged(() => State); }
        }

        private MusicLoader _musicLoader;

        public MainViewModel(MusicLoader musicLoader)
        {
            // TODO: Can we use some sort of eventing system so the managers layer doesn't have to know the viewmodel layer?
            _musicLoader = musicLoader;
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";
        }

        public ICommand OpenFileCommand => new RelayCommand(() =>
        {
			FileName = ChooseFile.getFileChoice();
        });

        public ICommand LoadCommand => new RelayCommand(() =>
        {
			ILoader loader = new LoaderFactory().createLoader(Path.GetExtension(FileName));
			string loadedText = loader.load(FileName);
			_musicLoader.LoadLilypondIntoWpfStaffsAndMidi(loadedText);
		});

		#region Focus and key commands, these can be used for implementing hotkeys
        public ICommand OnLostFocusCommand => new RelayCommand(() =>
        {
			keyHandler.LostFocus();
        });

        public ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
			keyHandler.KeyDown(e);
        });

        public ICommand OnKeyUpCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
			keyHandler.KeyUp(e);
		});

        public ICommand OnWindowClosingCommand => new RelayCommand(() =>
        {
            ViewModelLocator.Cleanup();
        });
		#endregion Focus and key commands, these can be used for implementing hotkeys
	}
}
