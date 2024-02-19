using BlinkShop.Web.Models;

namespace BlinkShop.Web.AsyncDataService.Interfaces
{
    public interface IMessageBusClient
    {
        void PublishCreateNewCoupon(CouponDto couponDto);
    }
}
