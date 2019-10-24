namespace totalKontrol.Core
{
    public abstract class HandlerResult
    {
        public bool ReflectEvent { get; set; }
    }

    public class SetLightOnResult : HandlerResult
    {
        public SetLightOnResult()
        {
            ReflectEvent = true;
        }
    }

    public class SetLightOffResult : HandlerResult
    {
        public SetLightOffResult()
        {
            ReflectEvent = true;
        }
    }
}
