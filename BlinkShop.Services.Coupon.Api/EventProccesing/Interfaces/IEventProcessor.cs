namespace BlinkShop.Services.Coupon.Api.EventProccesing.Interfaces
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}