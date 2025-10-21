using AutoMapper;
using Common.Enum;
using CoreValidation.Requests.Department;
using Helper.FileHelper;
using Helper.NLog;
using Infrastructure.Repositories;
using Object.Dto;
using Object.Model;
using Service.Service.RoleService;

namespace Service.Service.DepartmentService
{
    public interface IDepartmentService
    {
        List<DepartmentDto> GetAllDepartments(int userId);
        Task<bool> AddDepartment(AddDepartmentRequest request, int userId);
        Task<bool> UpdateDepartmentById(UpdateDepartmentRequest request, int userId);
        bool DeleteDepartmentById(int userId, int departmentId);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IBaseCommand<Department> _baseDepartmentCommand;
        private readonly IBaseCommand<User> _baseUserCommand;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;

        public DepartmentService(IBaseCommand<Department> baseDepartmentCommand, IBaseCommand<User> baseUserCommand, IRoleService roleService, IMapper mapper, IFileHelper fileHelper)
        {
            _baseDepartmentCommand = baseDepartmentCommand;
            _roleService = roleService;
            _mapper = mapper;
            _baseUserCommand = baseUserCommand;
            _fileHelper = fileHelper;
        }

        public List<DepartmentDto> GetAllDepartments(int userId)
        {
            try
            {
                List<DepartmentDto> departmentDtos = new List<DepartmentDto>();
                var user = _baseUserCommand.FindByCondition(x => x.UserId == userId && x.IsDeleted == false).FirstOrDefault();
                if (user == null)
                    throw new Exception("Người dùng không tồn tại");
                if (user.IsSuperAdmin == 1)
                {
                    var departments = _baseDepartmentCommand.FindByCondition(x => x.IsDeleted == false).ToList();
                    departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);
                }
                else if (user.RoleCode == "MANAGER")
                {
                    var departments = _baseDepartmentCommand.FindByCondition(x => x.DepartmentId == user.DepartmentId && x.IsDeleted == false).ToList();
                    departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);
                }
                else
                {
                    throw new Exception("Bạn không có quyền xem danh sách cơ sở");
                }
                return departmentDtos;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public async Task<bool> AddDepartment(AddDepartmentRequest request, int userId)
        {
            try
            {
                var newDepartment = new Department();
                newDepartment = _mapper.Map<Department>(request);
                newDepartment.CreateAt = DateTime.Now;
                newDepartment.CreateBy = userId;
                if (request.Photo != null)
                {
                    var relativePath = await _fileHelper.SaveAsync(request.Photo, "departments");

                    // Ghi đường dẫn vào DB
                    newDepartment.DepartmentPhoto = relativePath;
                }

                _baseDepartmentCommand.Create(newDepartment);
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public async Task<bool> UpdateDepartmentById(UpdateDepartmentRequest request, int userId)
        {
            try
            {
                var department = _baseDepartmentCommand.FindByCondition(x => x.DepartmentId == request.DepartmentId && x.IsDeleted == false).FirstOrDefault();
                if (department == null)
                {
                    throw new Exception("Không tìm thấy cơ sở này.");
                }
                department.DepartmentName = request.DepartmentName;
                department.DepartmentAddress = request.DepartmentAddress;
                department.UpdateAt = DateTime.Now;
                department.UpdateBy = userId;
                if (request.Photo != null)
                {
                    var relativePath = await _fileHelper.SaveAsync(request.Photo, "departments");

                    // Ghi đường dẫn vào DB
                    department.DepartmentPhoto = relativePath;
                }
                _baseDepartmentCommand.UpdateByEntity(department);
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public bool DeleteDepartmentById(int userId,int departmentId)
        {
            try
            {
                var department = _baseDepartmentCommand.FindByCondition(x => x.DepartmentId == departmentId && x.IsDeleted == false).FirstOrDefault();
                if (department == null)
                {
                    throw new Exception("Không tìm thấy cơ sở này.");
                }
                department.IsDeleted = true;
                department.UpdateBy = userId;
                department.UpdateAt = DateTime.Now;
                _baseDepartmentCommand.UpdateByEntity(department);
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }
    }
}
