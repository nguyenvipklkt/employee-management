using AutoMapper;
using CoreValidation.Requests.Table;
using Helper.FileHelper;
using Helper.NLog;
using Infrastructure.Repositories;
using Object.Model;

namespace Service.Service.TableService
{
    public interface ITableService
    {
        bool AddTable(int userId, AddTableRequest request);
        bool UpdateTable(int userId, UpdateTableRequest request);
        bool DeleteTable(int userId, int TableId);
    }
    public class TableService: ITableService
    {
        private readonly IBaseCommand<Table> _baseTableCommand;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;
        public TableService(IBaseCommand<Table> baseTableCommand, IMapper mapper, IFileHelper fileHelper)
        {
            _baseTableCommand = baseTableCommand;
            _mapper = mapper;
            _fileHelper = fileHelper;
        }
        public bool AddTable(int userId, AddTableRequest request)
        {
            try
            {
                var newTable = new Table();
                _mapper.Map(request, newTable);
                newTable.CreateBy = userId;
                var createdRole = _baseTableCommand.Create(newTable);
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }
        public bool UpdateTable (int userId, UpdateTableRequest request)
        {
            try
            {
                var existingTable = _baseTableCommand.FindOrFail(request.TableId);
                _mapper.Map(request, existingTable);
                existingTable.UpdateBy = userId;
                existingTable.UpdateAt = DateTime.Now;
                var updatedTable = _baseTableCommand.UpdateByEntity(existingTable);
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public bool DeleteTable(int userId, int TableId)
        {
            try
            {
                var existingTable = _baseTableCommand.FindOrFail(TableId);
                existingTable.UpdateAt = DateTime.Now;
                existingTable.UpdateBy = userId;
                existingTable.IsDeleted = true;
                var deletedTable = _baseTableCommand.UpdateByEntity(existingTable);
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
