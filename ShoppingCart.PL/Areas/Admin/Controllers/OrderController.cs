using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.BLL.Interfaces;
using ShoppingCart.BLL.Specifications;
using ShoppingCart.PL.Utilities;
using ShoppingCart.PL.ViewModels;
using Stripe;

namespace ShoppingCart.PL.Areas.Admin.Controllers
{
	[Authorize(Roles = UsersRoles.AdminRole)]
	[Area("Admin")]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public OrderController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		#region Index
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> GetOrdersData()
		{
			var specification = new OrderHeaderSpecification();

			var orderHeaders = await _unitOfWork.OrderHeaderRepository.GetAllWithSpecificationAsync(specification);

			return Json(new { data = orderHeaders });
		}
		#endregion

		#region Details
		public async Task<IActionResult> Details(int orderId)
		{
			var orderHeaderSpecification = new OrderHeaderSpecification(orderId);
			var orderDetailsSpecification = new OrderDetailsSpecification(orderId);

			var orderViewModel = new OrderViewModel
			{
				OrderHeader = await _unitOfWork.OrderHeaderRepository.GetEntityWithSpecificationAsync(orderHeaderSpecification),
				OrderDetails = await _unitOfWork.OrderDetailsRepository.GetAllWithSpecificationAsync(orderDetailsSpecification),
			};

			return View(orderViewModel);
		}
		#endregion

		#region UpdateOrderDetails
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateOrderDetails(OrderViewModel orderViewModel)
		{
			var orderFormDb = await _unitOfWork.OrderHeaderRepository.GetByIdAsync(orderViewModel.OrderHeader.Id);

			orderFormDb.UserName = orderViewModel.OrderHeader.UserName;
			orderFormDb.PhoneNumber = orderViewModel.OrderHeader.PhoneNumber;
			orderFormDb.City = orderViewModel.OrderHeader.City;
			orderFormDb.Address = orderViewModel.OrderHeader.Address;

			if (!string.IsNullOrEmpty(orderViewModel.OrderHeader.Carrier))
				orderFormDb.Carrier = orderViewModel.OrderHeader.Carrier;

			if (!string.IsNullOrEmpty(orderViewModel.OrderHeader.TrackingNumber))
				orderFormDb.TrackingNumber = orderViewModel.OrderHeader.TrackingNumber;

			_unitOfWork.OrderHeaderRepository.Update(orderFormDb);

			await _unitOfWork.CompleteAsync();

			TempData["Update"] = "Order Has Updated Successfully";

			return RedirectToAction("Details", "Order", new { orderId = orderFormDb.Id });
		}
		#endregion

		#region StartProcess
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> StartProcess(OrderViewModel orderViewModel)
		{
			_unitOfWork.OrderHeaderRepository.UpdateOrderStatus(orderViewModel.OrderHeader.Id, Status.Processing);

			await _unitOfWork.CompleteAsync();

			TempData["Update"] = "Order Status Has Updated Successfully";

			return RedirectToAction("Details", "Order", new { orderId = orderViewModel.OrderHeader.Id });
		}
		#endregion

		#region StartShip
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> StartShip(OrderViewModel orderViewModel)
		{
			var orderFormDb = await _unitOfWork.OrderHeaderRepository.GetByIdAsync(orderViewModel.OrderHeader.Id);

			orderFormDb.Carrier = orderViewModel.OrderHeader.Carrier;

			orderFormDb.TrackingNumber = orderViewModel.OrderHeader.TrackingNumber;

			orderFormDb.OrderStatus = Status.Shipped;

			orderFormDb.ShippingDate = DateTime.Now;

			_unitOfWork.OrderHeaderRepository.Update(orderFormDb);

			await _unitOfWork.CompleteAsync();

			TempData["Update"] = "Order Has Shipped Successfully";

			return RedirectToAction("Details", "Order", new { orderId = orderViewModel.OrderHeader.Id });
		}
		#endregion

		#region CancelOrder
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CancelOrder(OrderViewModel orderViewModel)
		{
			var orderFormDb = await _unitOfWork.OrderHeaderRepository.GetByIdAsync(orderViewModel.OrderHeader.Id);

			if (orderFormDb.PaymentStatus == Status.Approve)
			{
				var options = new RefundCreateOptions
				{
					Reason = RefundReasons.RequestedByCustomer,
					PaymentIntent = orderFormDb.PaymentIntentId,
				};

				var service = new RefundService();

				Refund refund = service.Create(options);

				_unitOfWork.OrderHeaderRepository.UpdateOrderStatus(orderFormDb.Id, Status.Canceled);
				_unitOfWork.OrderHeaderRepository.UpdatePaymentStatus(orderFormDb.Id, Status.Refund);
			}
			else
			{
				_unitOfWork.OrderHeaderRepository.UpdateOrderStatus(orderFormDb.Id, Status.Canceled);
				_unitOfWork.OrderHeaderRepository.UpdatePaymentStatus(orderFormDb.Id, Status.Canceled);
			}

			await _unitOfWork.CompleteAsync();

			TempData["Update"] = "Order Has Cancelled Successfully";

			return RedirectToAction("Details", "Order", new { orderId = orderViewModel.OrderHeader.Id });
		}
		#endregion
	}
}
