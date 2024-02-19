using BlinkShop.Web.AsyncDataService.Interfaces;
using BlinkShop.Web.Models;
using BlinkShop.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlinkShop.Web.Controllers;

public class CouponController : Controller
{
    private readonly ICouponService _couponService;
    private readonly IMessageBusClient _messageBusClient;

    public CouponController(ICouponService couponService
        , IMessageBusClient messageBusClient)
    {
        _couponService = couponService;
        _messageBusClient = messageBusClient;
    }
    [HttpGet]

    public async Task<IActionResult> CouponGetAll()
    {
        List<CouponDto> list = new();
        var response = await _couponService.GetAll();
        if (response != null && response.Success)
        {
            list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
        }

        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> CreateCoupon()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCoupon(CouponDto couponDto)
    {

        //var create = await _couponService.Create(couponDto);
        //if (create != null && create.Success)
        //{
        //    var result = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(create.Result));
        //    ViewBag.Message = "عملیات با موفقیت انجام شد";
        //    return View(couponDto);

        //}
        //ViewBag.Message = "عملیات با خطا  مواجه شد  ";


        _messageBusClient.PublishCreateNewCoupon(couponDto);
        return View();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Coponinfo(int id)
    {
        var response = await _couponService.GetById(id);
        if (response != null && response.Success)
        {
            CouponDto couponDto = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));

            return View(couponDto);
        }

        ViewBag.Message = "عملیات با خطا مواجه گردید";
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Deleted(CouponDto dto)
    {
        var response = await _couponService.Remove(dto.CouponId);
        if (response != null && response.Success)
        {
            return RedirectToAction("CouponGetAll");
        }

        return NotFound();
    }
}