using Application.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using System.Data;

namespace Application.Services
{
    public class DocumentTransferService : IDocumentTransferService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IDocumentTransferRepository _documentTransferRepository;
        private readonly ILogger<DocumentTransferService> _logger;
        private readonly IOptions<DocumentTransferConfig> _documentTransferConfig;
        private readonly string _directoryPath;

        public DocumentTransferService(IOptions<DocumentTransferConfig> documentTransferConfig, ITransactionRepository transactionRepository, 
            IDocumentTransferRepository documentTransferRepository, ILogger<DocumentTransferService> logger)
        {
            _documentTransferRepository = documentTransferRepository;
            _transactionRepository = transactionRepository;
            _documentTransferConfig = documentTransferConfig;
            _logger = logger;
            _directoryPath = _documentTransferConfig.Value.DocumentTransferDataPath;
        }


        public async Task ExecuteDocTransfer()
        {
            DocumentTransfer docTransfer = null;
            try
            {
                var docTransfers = await _documentTransferRepository.GetOpenedAsync();
                foreach (var transfer in docTransfers)
                {
                    docTransfer = transfer;

                    transfer.State = DocTransferState.InProcess;
                    transfer.Timestamp = DateTimeOffset.Now;
                    await _documentTransferRepository.UpdateAsync(transfer);

                    var directoryPath = CreateDirectory(transfer.Type, transfer.CreatedAt, transfer.UserId);

                    ExcelPackage.License.SetNonCommercialPersonal("My Name");
                    using var excelPackage = new ExcelPackage();
                    var fileName = $"{transfer.UserId}_{transfer.Type}_{transfer.CreatedAt:ddMMyyyyThh-mm-ss}.xlsx";
                    var filePath = Path.Combine(directoryPath, fileName);


                    if (transfer.Type == DocTransferType.PopularExpenses)
                    {
                        var expenseResult = await _transactionRepository.GetPopularExpenses(transfer.UserId, transfer.DateFrom, transfer.DateTo);
                        await CreateFilledExcelFilePopularExpenses(excelPackage, fileName, expenseResult);
                    }
                    else if (transfer.Type == DocTransferType.Tendency)
                    {
                        var tendencyResult = await _transactionRepository.GetTendency(transfer.UserId, transfer.DateFrom, transfer.DateTo);
                        await CreateFilledExcelFilePopularTendency(excelPackage, fileName, tendencyResult);
                    }


                    excelPackage.SaveAs(new FileInfo(filePath));

                    transfer.State = DocTransferState.Completed;
                    transfer.Timestamp = DateTimeOffset.Now;
                    await _documentTransferRepository.UpdateAsync(transfer);
                    return;
                }
            }
            catch (Exception e)
            {
                if (docTransfer != null)
                {
                    docTransfer.State = DocTransferState.Failed;
                    docTransfer.Timestamp = DateTimeOffset.Now;
                    await _documentTransferRepository.UpdateAsync(docTransfer);
                }
                _logger.LogWarning(e, "Can not make document transfer");
            }
        }

        private string CreateDirectory(DocTransferType type, DateTimeOffset createdAt, string userId)
        {
            _logger.LogInformation("Try create directories");
            var path = "";
            try
            {
                path = CreatePath(type, createdAt, userId);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    _logger.LogInformation($"Directory created by path: {path}");
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Exception create directories");
                throw;
            }

            return path;
        }

        private string CreatePath(DocTransferType type, DateTimeOffset createdAt, string userId)
        {
            var dateForPath = createdAt.ToString("ddMMyyyyThh-mm-ss");
            var fileName = $"{userId}_{type}_{createdAt:ddMMyyyyThh-mm-ss}.xlsx";
            return Path.GetFullPath(Path.Combine(_directoryPath, userId, type + "", dateForPath));
        }

        private async Task<bool> FileExists(DocTransferType type, DateTimeOffset createdAt, string userId)
        {
            var filePath = CreatePath(type, createdAt, userId);
            return File.Exists(filePath);
        }

        private async Task CreateFilledExcelFilePopularExpenses(ExcelPackage excelPackage, string fileName, IEnumerable<PopularExpense> popularExpenses)
        {
            try
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(fileName);
                _logger.LogDebug("Try create Excel file");
                var dataTable = new DataTable();
                dataTable.Columns.Add("Name", typeof(string));
                dataTable.Columns.Add("Total", typeof(string));

                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                foreach (var item in popularExpenses)
                {
                    dataTable.Rows.Add(
                        item.Name,
                        item.Total
                    );
                }

                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                _logger.LogDebug("Created excel file with data.");
            }
            catch (Exception e)
            {
                _logger.LogWarning("Exception create Excel file");
                throw e;
            }
        }

        private async Task CreateFilledExcelFilePopularTendency(ExcelPackage excelPackage, string fileName, IEnumerable<Tendency> tendencies)
        {
            try
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(fileName);
                _logger.LogDebug("Try create Excel file");
                var dataTable = new DataTable();
                dataTable.Columns.Add("Month", typeof(string));
                dataTable.Columns.Add("Income", typeof(string));
                dataTable.Columns.Add("Expense", typeof(string));

                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                foreach (var item in tendencies)
                {
                    dataTable.Rows.Add(
                        item.Month,
                        item.Income,
                        item.Expense
                    );
                }

                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                _logger.LogDebug("Created excel file with data.");
            }
            catch (Exception e)
            {
                _logger.LogWarning("Exception create Excel file");
                throw e;
            }
        }
    }
}
