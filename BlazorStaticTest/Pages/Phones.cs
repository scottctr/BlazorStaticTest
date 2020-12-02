using System;
using SwitchBoard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorStaticTest.Pages
{
    public partial class Phones
    {
        private readonly List<LogEntry> _log = new List<LogEntry>();

        public Phones()
        {
            StateMachine.OnChange += stateMachine_OnChange;

            _log.Add(new LogEntry("started"));
        }

        public async Task Start()
        {
            var phone = new Phone { Name = "Phone1", State = PhoneState.OnHook };
            var eventCount = Enum.GetNames<PhoneEvent>().Length;
            var randomizer = new Random();

            for (int i = 0; i < 222; i++)
            {
                await Task.Delay(1);
                var nextEvent = (PhoneEvent)(randomizer.Next(eventCount));
                //var nextEvent = Enum.GetValues<PhoneEvent>()[randomizer.Next(eventCount)];
                StateMachine.Fire(phone, nextEvent);
                StateHasChanged();
            }
        }

        private void stateMachine_OnChange(object sender, LogEntry logEntry)
        {
            _log.Add(logEntry);
        }
    }
}