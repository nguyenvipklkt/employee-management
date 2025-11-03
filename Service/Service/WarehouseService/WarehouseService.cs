using AutoMapper;
using Common.Enum.RoleEnum;
using Infrastructure.Repositories;
using Object.Dto;
using Object.Model;

namespace Service.Service.WarehouseService
{
    public interface IWarehouseService
    {
        List<WarehouseDto> GetWarehouseList(int userId);
        bool AddWarehouse(int userId, string warehouseName, int departmentId);
        bool UpdateWarehouse(int userId, int warehouseId, string warehouseName, int departmentId);
        bool DeleteWarehouse(int userId, int warehouseId);
    }
    public class WarehouseService: IWarehouseService
    {
        private readonly IBaseCommand<Warehouse> _baseWarehouseCommand;
        private readonly IBaseCommand<Department> _baseDepartmentCommand;
        private readonly IBaseCommand<User> _baseUserCommand;

        private readonly IMapper _mapper;

        public WarehouseService(IBaseCommand<Warehouse> baseWarehouseCommand, IBaseCommand<Department> baseDepartmentCommand, IBaseCommand<User> baseUserCommand, IMapper mapper)
        {
            _baseWarehouseCommand = baseWarehouseCommand;
            _baseDepartmentCommand = baseDepartmentCommand;
            _baseUserCommand = baseUserCommand;
            _mapper = mapper;
        }

        public List<WarehouseDto> GetWarehouseList(int userId)
        {
            var warehouseDtos = new List<WarehouseDto>();
            List<Warehouse> warehouses = new List<Warehouse>();
            try
            {
                var user = _baseUserCommand.FindByCondition(x => x.UserId == userId && x.IsDeleted == false).FirstOrDefault();
                if (user == null)
                    throw new Exception("Người dùng không tồn tại");
                if (user.IsSuperAdmin == 1)
                {
                    warehouses = _baseWarehouseCommand.FindAll().ToList();
                }
                else if (user.RoleCode == RoleEnum.MANAGER)
                {
                    warehouses = _baseWarehouseCommand.FindByCondition(x => x.DepartmentId == user.DepartmentId && x.IsDeleted == false).ToList();
                }
                else
                {
                    throw new Exception("Bạn không có quyền xem danh sách kho");
                }
                foreach (var warehouse in warehouses)
                {
                    var department = _baseDepartmentCommand.FindByCondition(x => x.DepartmentId == warehouse.DepartmentId).FirstOrDefault();
                    if (department == null)
                        throw new Exception("Cơ sở không tồn tại");
                    var warehouseDto = new WarehouseDto
                    {
                        WarehouseId = warehouse.WarehouseId,
                        WarehouseName = warehouse.Name,
                        DepartmentName = department.DepartmentName,
                        Address = department.DepartmentAddress,
                    };
                    warehouseDtos.Add(warehouseDto);
                }
                return warehouseDtos;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AddWarehouse(int userId, string warehouseName, int departmentId)
        {
            try
            {
                var department = _baseDepartmentCommand.FindByCondition(x => x.DepartmentId == departmentId).FirstOrDefault();
                if (department == null)
                    throw new Exception("Cơ sở không tồn tại");
                var newWarehouse = new Warehouse
                {
                    Name = warehouseName,
                    DepartmentId = departmentId,
                    CreateAt = DateTime.Now,
                    CreateBy = userId,
                };
                _baseWarehouseCommand.Create(newWarehouse);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateWarehouse(int userId, int warehouseId, string warehouseName, int departmentId)
        {
            try
            {
                var warehouse = _baseWarehouseCommand.FindByCondition(x => x.WarehouseId == warehouseId && x.IsDeleted == false).FirstOrDefault();
                if (warehouse == null)
                    throw new Exception("Kho không tồn tại");
                warehouse.Name = warehouseName;
                warehouse.DepartmentId = departmentId;
                warehouse.UpdateAt = DateTime.Now;
                warehouse.UpdateBy = userId;
                _baseWarehouseCommand.UpdateByEntity(warehouse);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteWarehouse(int userId, int warehouseId)
        {
            try
            {
                var warehouse = _baseWarehouseCommand.FindByCondition(x => x.WarehouseId == warehouseId && x.IsDeleted == false).FirstOrDefault();
                if (warehouse == null)
                    throw new Exception("Kho không tồn tại");
                warehouse.UpdateAt = DateTime.Now;
                warehouse.UpdateBy = userId;
                warehouse.IsDeleted = true;
                _baseWarehouseCommand.UpdateByEntity(warehouse);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
