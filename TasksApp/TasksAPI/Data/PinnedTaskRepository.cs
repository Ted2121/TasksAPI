using Dapper;
using System.Data.SqlClient;
using TasksAPI.Models;

namespace TasksAPI.Data;

public class PinnedTaskRepository : BaseDataAccess, IPinnedTaskRepository
{
    public PinnedTaskRepository(string connectionstring) : base(connectionstring)
    {
    }

    public async Task<bool> DeletePinnedTaskAsync(int pinnedTaskId)
    {
        string commandText = "DELETE FROM PinnedTask WHERE Id = @Id";
        try
        {
            using (SqlConnection connection = CreateConnection())
            {
                var parameters = new
                {
                    Id = pinnedTaskId
                };
                return await connection.ExecuteAsync(commandText, parameters) > 0;
            }

        }
        catch (Exception ex)
        {
            throw new Exception($"Exception while trying to delete a row from PinnedTask table. The exception was: '{ex.Message}'", ex);
        }
    }

    public async Task<IEnumerable<PinnedTask>> GetAllPinnedTasksAsync()
    {
        try
        {
            string commandText = "SELECT * FROM PinnedTask";
            using (SqlConnection connection = CreateConnection())
            {
                var pinnedTasks = await connection.QueryAsync<PinnedTask>(commandText);

                return pinnedTasks;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception while trying to read all rows from the PinnedTask table. The exception was: '{ex.Message}'", ex);
        }
    }

    public async Task<PinnedTask> GetPinnedTaskByIdAsync(int pinnedTaskId)
    {
        try
        {
            string commandText = "SELECT * FROM PinnedTask WHERE Id = @Id";
            using (SqlConnection connection = CreateConnection())
            {
                var parameters = new
                {
                    Id = pinnedTaskId
                };

                var pinnedTask = await connection.QuerySingleOrDefaultAsync<PinnedTask>(commandText, parameters);

                return pinnedTask;
            }
        }
        catch (Exception ex)
        {

            throw new($"Exception while trying to find the pinnedTask with the '{pinnedTaskId}'. The exception was: '{ex.Message}'", ex);


        }
    }

    public async Task<int> InsertPinnedTaskAsync(PinnedTask pinnedTask)
    {
        try
        {
            string commandText = "INSERT INTO PinnedTask (Text, LabelName, UserId) VALUES (@Text, @LabelName, @UserId); SELECT CAST(scope_identity() AS int)";
            using (SqlConnection connection = CreateConnection())
            {

                var parameters = new
                {
                    Text = pinnedTask.Text,
                    LabelName = pinnedTask.LabelName,
                    UserId = pinnedTask.UserId,
                };


                return pinnedTask.Id = await connection.QuerySingleAsync<int>(commandText, parameters);
            }
        }
        catch (Exception ex)
        {

            throw new($"Exception while trying to insert a PinnedTask object. The exception was: '{ex.Message}'", ex);

        }
    }

    public async Task<bool> UpdatePinnedTaskAsync(PinnedTask pinnedTask)
    {
        try
        {
            string commandText = "UPDATE PinnedTask SET " +
                "Text = @Text, " +
                "LabelName = @LabelName, " +
                "UserId = @UserId " +
                "WHERE Id = @Id";

            using (SqlConnection connection = CreateConnection())
            {
                var parameters = new
                {
                    Text = pinnedTask.Text,
                    LabelName = pinnedTask.LabelName,
                    UserId = pinnedTask.UserId,
                    Id = pinnedTask.Id
                };

                return await connection.ExecuteAsync(commandText, parameters) > 0;

            }
        }
        catch (Exception ex)
        {

            throw new Exception($"Exception while trying to update a PinnedTask. The exception was: '{ex.Message}'", ex);

        }
    }
}
