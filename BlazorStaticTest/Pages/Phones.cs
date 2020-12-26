using Excubo.Blazor.Diagrams;
using SwitchBoard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorStaticTest.Pages
{
    public partial class Phones
    {
        private readonly List<PhoneNode> nodes = new List<PhoneNode>
        {
            new PhoneNode { Phone = new Phone { Name = "Phone1" }},
            new PhoneNode { Phone = new Phone { Name = "Phone2" }}
        };
        private List<LinkData> links = new List<LinkData>();
        private AutoLayoutSettings auto_layout;
        private bool run_auto_layout = false;
        private readonly List<LogEntry> _log = new List<LogEntry>();
        private readonly List<PhoneEvent> _stateDistribution = new List<PhoneEvent>();

        public Phones()
        {
            StateMachine.OnChange += stateMachine_OnChange;
            setupStateDistribution();

            _log.Add(new LogEntry("started"));
            Console.WriteLine(StateMachine.GetDotGraph());
        }

        private void setupStateDistribution()
        {
            _stateDistribution.Add(PhoneEvent.IncomingCall);
            _stateDistribution.Add(PhoneEvent.IncomingCall);
            _stateDistribution.Add(PhoneEvent.IncomingCall);
            _stateDistribution.Add(PhoneEvent.IncomingCall);
            _stateDistribution.Add(PhoneEvent.IncomingCall);
            _stateDistribution.Add(PhoneEvent.IncomingCall);
            _stateDistribution.Add(PhoneEvent.IncomingCall);
            _stateDistribution.Add(PhoneEvent.IncomingCall);
            _stateDistribution.Add(PhoneEvent.IncomingCall);
            _stateDistribution.Add(PhoneEvent.IncomingCall);

            _stateDistribution.Add(PhoneEvent.Answer);
            _stateDistribution.Add(PhoneEvent.Answer);
            _stateDistribution.Add(PhoneEvent.Answer);
            _stateDistribution.Add(PhoneEvent.Answer);
            _stateDistribution.Add(PhoneEvent.Answer);
            _stateDistribution.Add(PhoneEvent.Answer);
            _stateDistribution.Add(PhoneEvent.Answer);
            _stateDistribution.Add(PhoneEvent.Answer);
            _stateDistribution.Add(PhoneEvent.Answer);
            _stateDistribution.Add(PhoneEvent.Answer);

            _stateDistribution.Add(PhoneEvent.PickUp);
            _stateDistribution.Add(PhoneEvent.PickUp);
            _stateDistribution.Add(PhoneEvent.PickUp);
            _stateDistribution.Add(PhoneEvent.PickUp);
            _stateDistribution.Add(PhoneEvent.PickUp);
            _stateDistribution.Add(PhoneEvent.PickUp);
            _stateDistribution.Add(PhoneEvent.PickUp);
            _stateDistribution.Add(PhoneEvent.PickUp);
            _stateDistribution.Add(PhoneEvent.PickUp);
            _stateDistribution.Add(PhoneEvent.PickUp);

            _stateDistribution.Add(PhoneEvent.Dialing);
            _stateDistribution.Add(PhoneEvent.Dialing);
            _stateDistribution.Add(PhoneEvent.Dialing);
            _stateDistribution.Add(PhoneEvent.Dialing);
            _stateDistribution.Add(PhoneEvent.Dialing);
            _stateDistribution.Add(PhoneEvent.Dialing);
            _stateDistribution.Add(PhoneEvent.Dialing);
            _stateDistribution.Add(PhoneEvent.Dialing);
            _stateDistribution.Add(PhoneEvent.Dialing);
            _stateDistribution.Add(PhoneEvent.Dialing);

            _stateDistribution.Add(PhoneEvent.DialingDone);
            _stateDistribution.Add(PhoneEvent.DialingDone);
            _stateDistribution.Add(PhoneEvent.DialingDone);
            _stateDistribution.Add(PhoneEvent.DialingDone);
            _stateDistribution.Add(PhoneEvent.DialingDone);
            _stateDistribution.Add(PhoneEvent.DialingDone);
            _stateDistribution.Add(PhoneEvent.DialingDone);
            _stateDistribution.Add(PhoneEvent.DialingDone);
            _stateDistribution.Add(PhoneEvent.DialingDone);
            _stateDistribution.Add(PhoneEvent.DialingDone);
            _stateDistribution.Add(PhoneEvent.DialingDone);

            _stateDistribution.Add(PhoneEvent.HangUp);
            _stateDistribution.Add(PhoneEvent.HangUp);
            _stateDistribution.Add(PhoneEvent.HangUp);
            _stateDistribution.Add(PhoneEvent.HangUp);
            _stateDistribution.Add(PhoneEvent.HangUp);
            _stateDistribution.Add(PhoneEvent.HangUp);
            _stateDistribution.Add(PhoneEvent.HangUp);
            _stateDistribution.Add(PhoneEvent.HangUp);


            _stateDistribution.Add(PhoneEvent.CallerBusy);
            _stateDistribution.Add(PhoneEvent.CallerBusy);
            _stateDistribution.Add(PhoneEvent.CallerBusy);
            _stateDistribution.Add(PhoneEvent.CallerBusy);
            _stateDistribution.Add(PhoneEvent.CallerBusy);


            _stateDistribution.Add(PhoneEvent.Ringing);
            _stateDistribution.Add(PhoneEvent.Ringing);
            _stateDistribution.Add(PhoneEvent.Ringing);
            _stateDistribution.Add(PhoneEvent.Ringing);
            _stateDistribution.Add(PhoneEvent.Ringing);
            _stateDistribution.Add(PhoneEvent.Ringing);
            _stateDistribution.Add(PhoneEvent.Ringing);
            _stateDistribution.Add(PhoneEvent.Ringing);
            _stateDistribution.Add(PhoneEvent.Ringing);
            _stateDistribution.Add(PhoneEvent.Ringing);

            _stateDistribution.Add(PhoneEvent.CallerPickedUp);
            _stateDistribution.Add(PhoneEvent.CallerPickedUp);
            _stateDistribution.Add(PhoneEvent.CallerPickedUp);
            _stateDistribution.Add(PhoneEvent.CallerPickedUp);
            _stateDistribution.Add(PhoneEvent.CallerPickedUp);
            _stateDistribution.Add(PhoneEvent.CallerPickedUp);
            _stateDistribution.Add(PhoneEvent.CallerPickedUp);
            _stateDistribution.Add(PhoneEvent.CallerPickedUp);
            _stateDistribution.Add(PhoneEvent.CallerPickedUp);
            _stateDistribution.Add(PhoneEvent.CallerPickedUp);

            _stateDistribution.Add(PhoneEvent.PutOnHold);
            _stateDistribution.Add(PhoneEvent.PutOnHold);
            _stateDistribution.Add(PhoneEvent.PutOnHold);

            _stateDistribution.Add(PhoneEvent.RemoveHold);
            _stateDistribution.Add(PhoneEvent.RemoveHold);
            _stateDistribution.Add(PhoneEvent.RemoveHold);
            _stateDistribution.Add(PhoneEvent.RemoveHold);
            _stateDistribution.Add(PhoneEvent.RemoveHold);
            _stateDistribution.Add(PhoneEvent.RemoveHold);

            _stateDistribution.Add(PhoneEvent.StartRecording);

            _stateDistribution.Add(PhoneEvent.StopRecording);
            _stateDistribution.Add(PhoneEvent.StopRecording);
            _stateDistribution.Add(PhoneEvent.StopRecording);
            _stateDistribution.Add(PhoneEvent.StopRecording);
            _stateDistribution.Add(PhoneEvent.StopRecording);
            _stateDistribution.Add(PhoneEvent.StopRecording);

            _stateDistribution.Add(PhoneEvent.RemoveFromService);

            _stateDistribution.Add(PhoneEvent.ReturnToService);
            _stateDistribution.Add(PhoneEvent.ReturnToService);
            _stateDistribution.Add(PhoneEvent.ReturnToService);
            _stateDistribution.Add(PhoneEvent.ReturnToService);
            _stateDistribution.Add(PhoneEvent.ReturnToService);
            _stateDistribution.Add(PhoneEvent.ReturnToService);

            _stateDistribution.Add(PhoneEvent.LineDisruption);
        }

        public async Task Start()
        {
            auto_layout.Run();

            var eventCount = _stateDistribution.Count;
            var randomizer = new Random();

            for (var i = 0; i < 222; i++)
            {
                await Task.Delay(1);

                nodes.ForEach(n =>
                {
                    var nextEvent = _stateDistribution[randomizer.Next(eventCount)];
                    StateMachine.Fire(n.Phone, nextEvent);
                    StateHasChanged();
                });

                if (i % 10 == 0)
                {
                    nodes.Add(new PhoneNode { Phone = new Phone { Name = "Phone" + i } });
                    //run_auto_layout = true;
                    auto_layout.Run();
                }
            }
        }

        private void stateMachine_OnChange(object sender, LogEntry logEntry)
        {
            _log.Add(logEntry);
        }

        private bool no_implicit_render;
        protected override bool ShouldRender()
        {
            if (no_implicit_render)
            {
                no_implicit_render = false;
                return false;
            }
            return base.ShouldRender();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (run_auto_layout)
            {
                auto_layout.Run();
                run_auto_layout = false;
            }
        }
    }
}