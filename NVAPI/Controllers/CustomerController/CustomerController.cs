using CoreValidation;
using CoreValidation.Requests.Customer;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Setting;
using Service.Service.CustomerService;

namespace NVAPI.Controllers.CustomerController
{
    [ApiController]
    [Route("api/customer")]
    [Authorize]
    public class CustomerController : BaseApiController<CustomerController>
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("get-customer-list")]
        public APIResponse GetCustomerList()
        {
            try
            {
                var res = _customerService.GetCustomerList();
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpGet]
        [Route("search-customers")]
        public APIResponse SearchCustomers([FromQuery] string? name, string? email)
        {
            try
            {
                var res = _customerService.SearchCustomers(UserId, name, email);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost]
        [Route("add-customer")]
        public APIResponse AddCustomer(AddCustomerRequest request, IValidator<AddCustomerRequest> validator)
        {
            try
            {
                ValidatorFunc.ValidateRequest(validator, request);
                var res = _customerService.AddCustomer(request, UserId);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPut]
        [Route("update-customer")]
        public APIResponse UpdateCustomer(UpdateCustomerRequest request, IValidator<UpdateCustomerRequest> validator)
        {
            try
            {
                ValidatorFunc.ValidateRequest(validator, request);
                var res = _customerService.UpdateCustomer(request, UserId);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpDelete]
        [Route("delete-customer")]
        public APIResponse DeleteCustomer([FromQuery] int customerId)
        {
            try
            {
                var res = _customerService.DeleteCustomer(customerId);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost]
        [Route("send-email-to-customers")]
        public APIResponse SendEmailToCustomers(List<int> customerIdList, string content)
        {
            try
            {
                var res = _customerService.SendEmailToCustomers(customerIdList, content);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
