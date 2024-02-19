using AutoMapper;
using BlinkShop.Services.Coupon.Api.Data.Context;
using BlinkShop.Services.Coupon.Api.EventProccesing.Interfaces;
using BlinkShop.Services.Coupon.Api.Models;
using System.Text.Json;

namespace BlinkShop.Services.Coupon.Api.EventProccesing.Implements
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IMapper mapper, IServiceScopeFactory scopeFactory) 
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            AddNewCoupon(message);

            //var eventType = DetermineEvent(message);

            //switch (eventType)
            //{
            //    case EventType.CreateNewCoupon:
            //        AddNewCoupon(message);
            //        break;
            //    default:
            //        break;
            //}
        }

        public void AddNewCoupon(string message)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<BlinkShopDbContext>();

                    CouponDto dto = JsonSerializer.Deserialize<CouponDto>(message);

                    var result = _mapper.Map<Models.Coupon>(dto);

                    dbContext.Add(result);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"could not add coupon ===> {ex.Message}");
            }
        }


        //private EventType DetermineEvent(string notifcationMessage)
        //{
        //    Console.WriteLine("--> Determining Event");

        //    var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);

        //    switch (eventType.Event)
        //    {
        //        case "CreateNewCoupon":
        //            Console.WriteLine("--> Create New Coupon Event Detected");
        //            return EventType.CreateNewCoupon;
        //        default:
        //            Console.WriteLine("--> Could not determine the event type");
        //            return EventType.Undetermined;
        //    }
        //}


        //enum EventType
        //{
        //    CreateNewCoupon,
        //    Undetermined
        //}

        //public class GenericEventDto
        //{
        //    public string Event { get; set; }
        //}
    }
}