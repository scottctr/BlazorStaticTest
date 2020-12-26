namespace SwitchBoard
{
    public class Phone
    {
        public string Name { get; set; }
        public PhoneState PrevState { get; set; } = PhoneState.OutOfService;
        public PhoneEvent LastEvent { get; set; } = PhoneEvent.RemoveFromService;
        public PhoneState State { get; set; } = PhoneState.OnHook;
    }
}