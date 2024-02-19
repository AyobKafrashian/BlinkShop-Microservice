using AutoMapper;
using BlinkShop.Services.Coupon.Api.Data.Context;
using BlinkShop.Services.Coupon.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BlinkShop.Services.Coupon.Api.Controllers;
[EnableCors("AllowsAll")]
public class CouponContorller : BaseOfController
{
    private readonly BlinkShopDbContext _blink;
    private ResponseDto _responseDto;
    private readonly IMapper _mapper;
    public CouponContorller(BlinkShopDbContext blink, IMapper mapper)
    {
        _blink = blink;
        _mapper = mapper;
        _responseDto = new ResponseDto();
    }

    [HttpGet]
    //[Authorize]
    public ResponseDto GetAll()
    {
        try
        {
            IEnumerable<Models.Coupon> objectlist = _blink.model.ToList();
            _responseDto.Result = _mapper.Map<IEnumerable<CouponDto>>(objectlist);
            return _responseDto;
        }
        catch (Exception e)
        {
            _responseDto.Success=false;
            _responseDto.Massege = e.Message;
            return _responseDto;
        }
    }
    [HttpGet]
    [Route("{id:int}")]
    public ResponseDto GetById(int id)
    {
        try
        {
            Models.Coupon Couponobject = _blink.model.FirstOrDefault(x => x.CouponId == id);
            _responseDto.Result = _mapper.Map<CouponDto>(Couponobject);
            return _responseDto;
        }
        catch (Exception e)
        {
            _responseDto.Success=false;
            _responseDto.Massege = e.Message;
            return _responseDto;
        }
    }
    [HttpGet]
    [Route("{code}")]
    public ResponseDto GetByCode(string code)
    {
        try
        {
            Models.Coupon Couponobject = _blink.model.First(x => x.CouponCode.ToLower() == code.ToLower());
            _responseDto.Result = _mapper.Map<CouponDto>(Couponobject);
            return _responseDto;
        }
        catch (Exception e)
        {
            _responseDto.Success=false;
            _responseDto.Massege = e.Message;
            return _responseDto;
        }
    }

    [HttpPost]
    public ResponseDto CreateCoupon([FromBody]CouponDto couponDto)
    
    {
        try
        {
            var result = _mapper.Map<Models.Coupon>(couponDto);

            _blink.Add(result);
            _blink.SaveChanges();
            _responseDto.Result = couponDto;
            return _responseDto;
        }
        catch (Exception e)
        {
            _responseDto.Success=false;
            _responseDto.Massege = e.Message;
            return _responseDto;
        }
    }
    
    [HttpPut]
    public ResponseDto UpdateCoupon([FromBody]CouponDto couponDto)
    {
        try
        {
            var result = _mapper.Map<Models.Coupon>(couponDto);

            _blink.model.Update(result);
            _blink.SaveChanges();
            _responseDto.Result = couponDto;
            return _responseDto;
        }
        catch (Exception e)
        {
            _responseDto.Success=false;
            _responseDto.Massege = e.Message;
            return _responseDto;
        }
    }
    
    [HttpDelete("{id:int}")]
    public ResponseDto UpdateCoupon(int id)
    {
        try
        {
            var result = _blink.model.First(x => x.CouponId == id);

            _blink.model.Remove(result);
            _blink.SaveChanges();
            _responseDto.Result = result;
            return _responseDto;
        }
        catch (Exception e)
        {
            _responseDto.Success=false;
            _responseDto.Massege = e.Message;
            return _responseDto;
        }
    }
    
}