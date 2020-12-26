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

        public static string GetDotGraph()
        {
            return DotGraphExporter<PhoneState, PhoneEvent>.Export(_stateMachine.GetSummary());

        }

        private static void configure()
        {
            _stateMachine.OnTriggerNotConfigured += (sender, args) =>
            {
                var logEntry = new LogEntry($"{args.Parameters.Context.Name} not configured to transition from {args.Parameters.Context.State} on {args.Parameters.Trigger}");
                OnChange?.Invoke(args.Parameters.Context, logEntry);
            };

            _stateMachine.OnTransition += (sender, args) =>
            {
                args.Parameters.Context.PrevState = args.TransitionResult.StartingState;
                args.Parameters.Context.LastEvent = args.Parameters.Trigger;
                var logEntry = new LogEntry($"{args.Parameters.Context.Name} transitioned from {args.TransitionResult.StartingState} to {args.Parameters.Context.State} on {args.TransitionResult.Trigger}");
                OnChange?.Invoke(args.Parameters.Context, logEntry);
            };

           var onHookConfig = _stateMachine.ConfigureState(PhoneState.OnHook)
               //??? 1st couple of transitions are hokey since they don't apply to InRinging
               .AddTransition(PhoneEvent.IncomingCall, PhoneState.InRinging, phone => phone.State == PhoneState.OnHook)
               .AddTransition(PhoneEvent.PickUp, PhoneState.ReadyToDial, phone => phone.State == PhoneState.OnHook)
               .AddTransition(PhoneEvent.HangUp, PhoneState.OnHook)
               .AddTransition(PhoneEvent.LineDisruption, PhoneState.OutOfService)
               .AddTransition(PhoneEvent.RemoveFromService, PhoneState.OutOfService);

           _stateMachine.ConfigureState(PhoneState.InRinging)
              .MakeSubStateOf(onHookConfig)
              .AddTransition(PhoneEvent.Answer, PhoneState.Connected);

            var offHookConfig = _stateMachine.ConfigureState(PhoneState.OffHook)
               .AddTransition(PhoneEvent.HangUp, PhoneState.OnHook)
               .AddTransition(PhoneEvent.LineDisruption, PhoneState.OutOfService)
               .AddTransition(PhoneEvent.RemoveFromService, PhoneState.OutOfService);

            _stateMachine.ConfigureState(PhoneState.ReadyToDial)
               .MakeSubStateOf(offHookConfig)
               .AddTransition(PhoneEvent.Dialing, PhoneState.Dialing);

            _stateMachine.ConfigureState(PhoneState.Dialing)
               .MakeSubStateOf(offHookConfig)
               .AddTransition(PhoneEvent.DialingDone, PhoneState.GettingCallerStatus);

            _stateMachine.ConfigureState(PhoneState.GettingCallerStatus)
               .MakeSubStateOf(offHookConfig)
               .AddTransition(PhoneEvent.CallerBusy, PhoneState.Busy)
               .AddTransition(PhoneEvent.Ringing, PhoneState.OutRinging);

            _stateMachine.ConfigureState(PhoneState.Busy)
               .MakeSubStateOf(offHookConfig);

            _stateMachine.ConfigureState(PhoneState.OutRinging)
               .MakeSubStateOf(offHookConfig)
               .AddTransition(PhoneEvent.CallerPickedUp, PhoneState.Connected);

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
        }
    }
}