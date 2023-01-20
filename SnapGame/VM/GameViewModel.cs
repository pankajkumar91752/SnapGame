using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using SnapGame.Model;

namespace SnapGame.VM
{


    public class GameViewModel : INPCbase
    {

        DispatcherTimer timer = new DispatcherTimer();
        public GameContrSnapoller SnapGame { get; set; }
        public GameViewModel()
        {
            SnapGame = new GameContrSnapoller(Options);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            StartCommand = new RelayCommand(Reset);
            StopCommand = new RelayCommand(Stop, _ => SnapGame.State == GameState.Running);
            //StopCommand = new RelayCommand(Stop);
            ResetCommand = new RelayCommand(Reset);
            SnapGame.PropertyChanged += SnapGame_PropertyChanged;
        }

        private void SnapGame_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            StopCommand.RaiseCanExecChanged(sender, e);
            
            if (e.PropertyName != nameof(SnapGame.Result)) return;
            this.Log += SnapGame.Result;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (SnapGame.State == GameState.Running)
                SnapGame.Move();
            else
                End();
        }

        private void End()
        {
            SnapGame.End();
            this.Stop();
        }

        private void Reset(object obj = null)
        {
            this.Stop(obj);
            this.Deal();
            this.Start();
        }

        public void Deal() => SnapGame.Deal();

        public void Stop(object obj = null)
        {
            timer.Stop();
            SnapGame.Stop();
        }

        public void Start(object obj = null)
        {
            timer.Start();
            SnapGame.Start();
        }

        public string Name { get; set; }
        public GameOptions Options { get; set; } = new GameOptions();
      
        public RelayCommand StartCommand { get; set; }

        public RelayCommand StopCommand { get; set; }
        internal RelayCommand ResetCommand { get; private set; }
        public string Log { get; private set; }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> execHandler;
        private readonly Func<object,bool> canExecHandler;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<Object> execHandler,Func<object,bool> canExecHandler = null)
        {
            this.execHandler = execHandler;
            this.canExecHandler = canExecHandler;
            //CommandManager.RequerySuggested += (o, s) => RaiseCanExecChanged(o, s);
        }

        public void RaiseCanExecChanged(object? o, EventArgs s)
        {
            CanExecuteChanged?.Invoke(o, s);
        }

        public bool CanExecute(object parameter) => canExecHandler?.Invoke(parameter) ?? true;
        public void Execute(object? parameter) => execHandler.Invoke(parameter);
    }

   
}
