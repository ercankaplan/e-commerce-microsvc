using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    //BUSSINESS LAYER
    public class DiscountService(DiscountContext discountContext, ILogger<DiscountService> logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {

        public override Task<DiscountListResponse> DiscountList(DiscountListRequest request, ServerCallContext context)
        {
            var coupons = discountContext.Coupons.Take(request.Pagesize).ToList().Select(x => x.Adapt<CouponModel>()).ToList();

            var response = new DiscountListResponse() { Coupons = { coupons } };

            return Task.FromResult(response);

        }
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var couponEntity = await discountContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (couponEntity is null)
                return new CouponModel() { ProductName = "No Discount", Amount = 0, Description = "Empty" };

            logger.LogInformation("GetDiscount called with request: {@request} , response :{@response}",
                System.Text.Json.JsonSerializer.Serialize(request),
                System.Text.Json.JsonSerializer.Serialize(couponEntity));

            var couponModel = couponEntity.Adapt<CouponModel>();

            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request Model"));

            await discountContext.Coupons.AddAsync(coupon);

            await discountContext.SaveChangesAsync();

            logger.LogInformation("Created discount successfully request: {@request}", System.Text.Json.JsonSerializer.Serialize(request));

            return coupon.Adapt<CouponModel>();

        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {

            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request Model"));

            //var couponEntity = await discountContext.Coupons.FirstOrDefaultAsync(x => x.Id == request.Coupon.Id);

            //if (couponEntity == null)
            //    throw new RpcException(new Status(StatusCode.NotFound, "Coupon is not found"));

            var response = discountContext.Coupons.Update(coupon);

            await discountContext.SaveChangesAsync();

            logger.LogInformation("Updated discount successfully request: {@request}", System.Text.Json.JsonSerializer.Serialize(request));

            return response.Entity.Adapt<CouponModel>();

        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var couponEntity = await discountContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (couponEntity == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Coupon is not found"));
    

            discountContext.Coupons.Remove(couponEntity);

            await discountContext.SaveChangesAsync();

            logger.LogInformation("Deleted discount successfully request: {@request}", System.Text.Json.JsonSerializer.Serialize(request));

            return new DeleteDiscountResponse() { Success = true };
        }
    }
}
