using AutoMapper;
using Common.Enum.ErrorEnum;
using CoreValidation.Requests.Department;
using Helper.FileHelper;
using Helper.NLog;
using Infrastructure.Repositories;
using Object.Dto;
using Object.Model;

namespace Service.Service.DepartmentService
{
    public interface IDepartmentService
    {
        List<DepartmentDto> GetAllDepartments();
        Task<bool> AddDepartment(AddDepartmentRequest request, int userId);
        bool GrantManagerToDepartment(int departmentId, int managerId, int userId);
        bool RevokeManagerFromDepartment(int departmentId, int userId);
        Task<bool> UpdateDepartmentById(UpdateDepartmentRequest request, int userId);
        bool DeleteDepartmentById(int departmentId);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IBaseCommand<Department> _baseDepartmentCommand;
        private readonly IBaseCommand<User> _baseUserCommand;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;

        public DepartmentService(IBaseCommand<Department> baseDepartmentCommand, IBaseCommand<User> baseUserCommand, IMapper mapper, IFileHelper fileHelper)
        {
            _baseDepartmentCommand = baseDepartmentCommand;
            _mapper = mapper;
            _baseUserCommand = baseUserCommand;
            _fileHelper = fileHelper;
        }

        public List<DepartmentDto> GetAllDepartments()
        {
            try
            {
                List<DepartmentDto> departmentDtos = new List<DepartmentDto>();
                var departments = _baseDepartmentCommand.FindByCondition(x => true).ToList();
                if (departments == null || departments.Count == 0)
                {
                    return new List<DepartmentDto>();
                }
                foreach (var dept in departments)
                {
                    if (dept.ManagerId != null && dept.ManagerId != -1)
                    {
                        DepartmentDto dto = new DepartmentDto();
                        _mapper.Map(dept, dto);
                        var manager = _baseUserCommand.FindByCondition(x => x.UserId == dept.ManagerId).FirstOrDefault();
                        if (manager != null)
                        {
                            dto.UserName = manager.Name;
                        }
                        else
                        {
                            dto.UserName = "Chưa có quản lý";
                        }
                        departmentDtos.Add(dto);
                    }
                    else
                    {
                        DepartmentDto dto = new DepartmentDto();
                        _mapper.Map(dept, dto);
                        dto.UserName = "Chưa có quản lý";
                        departmentDtos.Add(dto);
                    }
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

        public bool GrantManagerToDepartment(int departmentId, int managerId, int userId)
        {
            try
            {
                var department = _baseDepartmentCommand.FindByCondition(x => x.DepartmentId == departmentId).FirstOrDefault();
                if (department == null)
                {
                    throw new Exception("Không tìm thấy cơ sở này."); // Department not found
                }
                department.ManagerId = managerId;
                department.UpdateAt = DateTime.Now;
                department.UpdateBy = userId;
                _baseDepartmentCommand.UpdateByEntity(department);
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public bool RevokeManagerFromDepartment(int departmentId, int userId)
        {
            try
            {
                var department = _baseDepartmentCommand.FindByCondition(x => x.DepartmentId == departmentId).FirstOrDefault();
                if (department == null)
                {
                    throw new Exception("Không tìm thấy cơ sở này.");
                }
                department.ManagerId = -1;
                department.UpdateAt = DateTime.Now;
                department.UpdateBy = userId;
                _baseDepartmentCommand.UpdateByEntity(department);
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
                var department = _baseDepartmentCommand.FindByCondition(x => x.DepartmentId == request.DepartmentId).FirstOrDefault();
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

        public bool DeleteDepartmentById(int departmentId)
        {
            try
            {
                var department = _baseDepartmentCommand.FindByCondition(x => x.DepartmentId == departmentId).FirstOrDefault();
                if (department == null)
                {
                    throw new Exception("Không tìm thấy cơ sở này.");
                }
                department.IsDeleted = true;
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
