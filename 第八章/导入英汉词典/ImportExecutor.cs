using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using SignalRCoreTest2;
using SignalRCoreTest3;
using System.Data;
using System.Data.SqlClient;

public class ImportExecutor
{
    private readonly IOptions<ConnStrOptions> optionsConnStr;
    private readonly IHubContext<ImportDictHub> hubContext;
    private readonly ILogger<ImportExecutor> logger;
    public ImportExecutor(IOptions<ConnStrOptions> optionsConnStr,
        IHubContext<ImportDictHub> hubContext, ILogger<ImportExecutor> logger)
    {
        this.optionsConnStr = optionsConnStr;
        this.hubContext = hubContext;
        this.logger = logger;
    }

    public async Task ExecuteAsync(string connectionId)
    {
        try
        {
            await DoExecuteAsync(connectionId);
        }
        catch (Exception ex)
        {
            await hubContext.Clients.Client(connectionId).SendAsync("Failed");
            logger.LogError(ex, "ImportExecutor出现异常");
        }
    }

    public async Task DoExecuteAsync(string connectionId)
    {
        //Hub方法超时时间很短，所以要放到线程中执行导入
        //用UserId进行客户端过滤的好处：退出后重新进来仍然能看到进度，坏处：不能并发多个任务
        //用ConnectionId过滤客户端的好处：可以多个并发，坏处：推出后再进来就看不到了。
        string[] lines = await File.ReadAllLinesAsync("d:/stardict.csv");//读取文件
        var client = hubContext.Clients.Client(connectionId);
        await client.SendAsync("Started");
        string connStr = optionsConnStr.Value.Default;//读取连接字符串
        using SqlConnection conn = new SqlConnection(connStr);
        await conn.OpenAsync();
        using SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
        bulkCopy.DestinationTableName = "T_WordItems";
        bulkCopy.ColumnMappings.Add("Word", "Word");
        bulkCopy.ColumnMappings.Add("Phonetic", "Phonetic");
        bulkCopy.ColumnMappings.Add("Definition", "Definition");
        bulkCopy.ColumnMappings.Add("Translation", "Translation");
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("Word");
        dataTable.Columns.Add("Phonetic");
        dataTable.Columns.Add("Definition");
        dataTable.Columns.Add("Translation");
        int count = lines.Length;
        for (int i = 1; i < count; i++)//跳过表头
        {
            string line = lines[i];
            string[] segments = line.Split(',');
            string word = segments[0];
            string? phonetic = segments[1];
            string? definition = segments[2];
            string? translation = segments[3];
            var dataRow = dataTable.NewRow();
            dataRow["Word"] = word;
            dataRow["Phonetic"] = phonetic;
            dataRow["Definition"] = definition;
            dataRow["Translation"] = translation;
            dataTable.Rows.Add(dataRow);
            if (dataTable.Rows.Count == 1000)
            {
                await bulkCopy.WriteToServerAsync(dataTable);
                dataTable.Clear();
                await client.SendAsync("ImportProgress", i, count);
            }
        }
        await client.SendAsync("ImportProgress", count, count);
        await bulkCopy.WriteToServerAsync(dataTable);//处理剩余的一组
        await client.SendAsync("Completed");
    }
}