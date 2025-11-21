using AutoMapper;
using CoreValidation.Requests.Customer;
using Helper.EmailHelper;
using Helper.NLog;
using Infrastructure.Repositories;
using Object.Model;
using System.Globalization;

namespace Service.Service.CustomerService
{
    public interface ICustomerService
    {
        List<Customer> GetCustomerList();
        List<Customer> SearchCustomers(int UserId, string? name, string? email);
        int AddCustomer(AddCustomerRequest request, int userId);
        int UpdateCustomer(UpdateCustomerRequest request, int userId);
        int DeleteCustomer(int customerId);
        Task<string> SendEmailToCustomers(List<int> customerIdList, string content);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IBaseCommand<User> _baseUserCommand;
        private readonly IBaseCommand<Customer> _baseCustomerCommand;
        private readonly IMapper _mapper;
        private readonly EmailHelper _emailHelper;

        public CustomerService(IBaseCommand<User> baseUserCommand, IBaseCommand<Customer> baseCustomerCommand, IMapper mapper, EmailHelper emailHelper)
        {
            _baseUserCommand = baseUserCommand;
            _baseCustomerCommand = baseCustomerCommand;
            _mapper = mapper;
            _emailHelper = emailHelper;
        }

        public List<Customer> GetCustomerList()
        {
            try
            {
                var customers = _baseCustomerCommand.FindAll().ToList();
                return customers;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public List<Customer> SearchCustomers(int UserId, string? name, string? email)
        {
            try
            {
                var query = _baseCustomerCommand.FindAll();
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(c => c.Name.Contains(name));
                }
                if (!string.IsNullOrEmpty(email))
                {
                    query = query.Where(c => c.Email.Contains(email));
                }
                return query.ToList();
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public int AddCustomer(AddCustomerRequest request, int userId)
        {
            try
            {
                DateTime? dob = null;

                if (!string.IsNullOrWhiteSpace(request.Dob))
                {
                    if (DateTime.TryParseExact(
                            request.Dob,
                            "dd/MM/yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateTime parsedDob))
                    {
                        dob = parsedDob;
                    }
                    else
                    {
                        dob = null;
                    }
                }

                var newCustomer = new Customer()
                {
                    Name = request.Name,
                    Email = request.Email,
                    Address = request.Address,
                    Dob = dob,
                    CreateBy = userId
                };

                _baseCustomerCommand.Create(newCustomer);
                return 1;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }


        public int UpdateCustomer(UpdateCustomerRequest request, int userId)
        {
            try
            {
                var existingCustomer = _baseCustomerCommand.FindOrFail(request.CustomerId);
                if (existingCustomer == null)
                {
                    throw new Exception("Khách hàng không tồn tại");
                }
                DateTime? dob = null;

                if (!string.IsNullOrWhiteSpace(request.Dob))
                {
                    if (DateTime.TryParseExact(
                            request.Dob,
                            "dd/MM/yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateTime parsedDob))
                    {
                        dob = parsedDob;
                    }
                    else
                    {
                        dob = null;
                    }
                }

                existingCustomer.Name = request.Name;
                existingCustomer.Email = request.Email;
                existingCustomer.Address = request.Address;
                existingCustomer.Dob = dob;
                existingCustomer.UpdateBy = userId;
                existingCustomer.UpdateAt = DateTime.Now;

                // Lưu DB
                _baseCustomerCommand.UpdateByEntity(existingCustomer);

                return 1;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }


        public int DeleteCustomer(int customerId)
        {
            try
            {
                var existingCustomer = _baseCustomerCommand.FindOrFail(customerId);
                if (existingCustomer == null)
                {
                    throw new Exception("Khách hàng không tồn tại");
                }
                _baseCustomerCommand.DeleteByEntity(existingCustomer);
                return 1;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public async Task<string> SendEmailToCustomers(List<int> customerIdList, string content)
        {
            List<Customer> customers = new List<Customer>();
            try
            {
                for (int i = 0; i < customerIdList.Count; i++)
                {
                    var customer = _baseCustomerCommand.FindOrFail(customerIdList[i]);
                    if (customer == null)
                    {
                        throw new Exception("Khách hàng không tồn tại");
                    }
                    customers.Add(customer);
                }
                string result = await _emailHelper.SendVerificationEmail(customers, content);
                return result;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }
    }
}
