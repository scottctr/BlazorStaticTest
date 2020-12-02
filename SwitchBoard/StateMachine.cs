using NStateManager.Export;
using NStateManager.Sync;
using System;

namespace SwitchBoard
{
    public static class StateMachine
    {
        private static readonly StateMachine<Phone, PhoneState, PhoneEvent> _stateMachine = new StateMachine<Phone, PhoneState, PhoneEvent>(
            phone => phone.State,
            (phone, state) => phone.State = state);

        static StateMachine()
        {
            configure();
        }

        public static void Fire(Phone phone, PhoneEvent @event)
        {
            _stateMachine.FireTrigger(phone, @event);
        }

        public static event EventHandler<LogEntry> OnChange; 

        private static void configure()
        {
            _stateMachine.OnTriggerNotConfigured += (sender, args) =>
            {
                var logEntry = new LogEntry($"{args.Parameters.Context.Name} not configured to transition from {args.Parameters.Context.State} on {args.Parameters.Trigger}");
                OnChange?.Invoke(args.Parameters.Context, logEntry);
            };

            _stateMachine.OnTransition += (sender, args) =>
            {
                var logEntry = new LogEntry($"{args.Parameters.Context.Name} transitioned from {args.TransitionResult.StartingState} to {args.Parameters.Context.State} on {args.TransitionResult.Trigger}");
                OnChange?.Invoke(args.Parameters.Context, logEntry);
            };

            _stateMachine.ConfigureState(PhoneState.OnHook)
               .AddTransition(PhoneEvent.PickUp, PhoneState.OffHook)
               .AddTransition(PhoneEvent.RemoveFromService, PhoneState.OutOfService);

            var offHookConfig = _stateMachine.ConfigureState(PhoneState.OffHook)
               .AddTransition(PhoneEvent.Connected, PhoneState.Connected)
               .AddTransition(PhoneEvent.HangUp, PhoneState.OnHook)
               .AddTransition(PhoneEvent.LineDisruption, PhoneState.OutOfService)
               .AddTransition(PhoneEvent.RemoveFromService, PhoneState.OutOfService);

            var connectedState = _stateMachine.ConfigureState(PhoneState.Connected)
               .MakeSubStateOf(offHookConfig)
               .AddTransition(PhoneEvent.PutOnHold, PhoneState.OnHold)
               .AddTransition(PhoneEvent.StartRecording, PhoneState.Recording);

            _stateMachine.ConfigureState(PhoneState.OnHold)
               .MakeSubStateOf(connectedState)
               .AddTransition(PhoneEvent.RemoveHold, PhoneState.Connected);

            _stateMachine.ConfigureState(PhoneState.Recording)
               .MakeSubStateOf(connectedState)
               .AddTransition(PhoneEvent.StopRecording, PhoneState.Connected);

            _stateMachine.ConfigureState(PhoneState.OutOfService)
               .AddTransition(PhoneEvent.ReturnToService, PhoneState.OnHook); //!!!

            OnChange?.Invoke(null, new LogEntry(DotGraphExporter<PhoneState, PhoneEvent>.Export(_stateMachine.GetSummary())));
        }
    }
}