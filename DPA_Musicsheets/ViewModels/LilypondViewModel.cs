﻿using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Managers.Commands;
using DPA_Musicsheets.States;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase
    {
        private MusicLoader _musicLoader;
        private MainViewModel _mainViewModel { get; set; }
		private LilypondText _text;
		private LilypondTextCaretaker _caretaker;
		private bool _movedInHistory;

        /// <summary>
        /// This text will be in the textbox.
        /// It can be filled either by typing or loading a file so we only want to set previoustext when it's caused by typing.
        /// </summary>
        public string LilypondText
        {
            get
            {
                return _text.Text;
            }
            set
            {
				_text.Text = value;
				RaisePropertyChanged(() => LilypondText);
            }
        }

        private bool _textChangedByLoad = false;
        private DateTime _lastChange;
        private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;



        public LilypondViewModel(MainViewModel mainViewModel, MusicLoader musicLoader)
        {
            // TODO: Can we use some sort of eventing system so the managers layer doesn't have to know the viewmodel layer and viewmodels don't know each other?
            // And viewmodels don't 
            _mainViewModel = mainViewModel;
            _musicLoader = musicLoader;
            _musicLoader.LilypondViewModel = this;

			_text = new LilypondText();
			_caretaker = new LilypondTextCaretaker(_text);
			_movedInHistory = false;

			LilypondText = "Your lilypond text will appear here.";
			_caretaker.change();
			UndoCommand.RaiseCanExecuteChanged();
			RedoCommand.RaiseCanExecuteChanged();
		}

        public void LilypondTextLoaded(string text)
        {
            _textChangedByLoad = true;
			_caretaker.reset();
			LilypondText = text;
			_caretaker.change();
			UndoCommand.RaiseCanExecuteChanged();
			RedoCommand.RaiseCanExecuteChanged();
			_textChangedByLoad = false;
        }

        /// <summary>
        /// This occurs when the text in the textbox has changed. This can either be by loading or typing.
        /// </summary>
        public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>((args) =>
        {
            Console.WriteLine("I am typing");
            // If we were typing, we need to do things.
            if (!_textChangedByLoad)
            {
                _mainViewModel.State = new EditedState();
                _lastChange = DateTime.Now;

                _mainViewModel.CurrentState = "Rendering...";

                Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith((task) =>
                {
                    if ((DateTime.Now - _lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
                    {
						// An undo or a redo (a 'move in history') should not be treated as a timeline change.
						if (!_movedInHistory) {
							_caretaker.change();
						} else {
							_movedInHistory = false;
						}
						UndoCommand.RaiseCanExecuteChanged();
						RedoCommand.RaiseCanExecuteChanged();

						_musicLoader.LoadLilypondIntoWpfStaffsAndMidi(LilypondText);
                        _mainViewModel.CurrentState = "";
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext()); // Request from main thread.
            }
        });

		#region Commands for buttons like Undo, Redo and SaveAs
		public RelayCommand UndoCommand => new RelayCommand(() =>
        {
			_movedInHistory = true;
			_caretaker.undo();
			RaisePropertyChanged(() => LilypondText);
			
		}, () => _caretaker.canUndo());

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
			_movedInHistory = true;
			_caretaker.redo();
			RaisePropertyChanged(() => LilypondText);
			
        }, () => _caretaker.canRedo());

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
			new CommandHandler().handleCommand("save", null);
        });
        #endregion Commands for buttons like Undo, Redo and SaveAs
    }
}
