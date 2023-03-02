using Dapper;
using System.Data.SqlClient;
using TasksAPI.Models;

namespace TasksAPI.Data;

public class SuggestedTaskRepository : BaseDataAccess, ISuggestedTaskRepository
{
    public SuggestedTaskRepository(string connectionstring) : base(connectionstring)
    {
    }

    public async Task<bool> DeleteSuggestedTaskAsync(int suggestedTaskId)
    {
        string commandText = "DELETE FROM SuggestedTask WHERE Id = @Id";
        try
        {
            using (SqlConnection connection = CreateConnection())
            {
                var parameters = new
                {
                    Id = suggestedTaskId
                };
                return await connection.ExecuteAsync(commandText, parameters) > 0;
            }

        }
        catch (Exception ex)
        {
            throw new Exception($"Exception while trying to delete a row from SuggestedTask table. The exception was: '{ex.Message}'", ex);
        }
    }

    public async Task<IEnumerable<SuggestedTask>> GetAllSuggestedTasksAsync()
    {
        try
        {
            string commandText = "SELECT * FROM SuggestedTask";
            using (SqlConnection connection = CreateConnection())
            {
                var suggestedTasks = await connection.QueryAsync<SuggestedTask>(commandText);

                return suggestedTasks;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception while trying to read all rows from the SuggestedTask table. The exception was: '{ex.Message}'", ex);
        }
    }

    public async Task<SuggestedTask> GetSuggestedTaskByIdAsync(int suggestedTaskId)
    {
        try
        {
            string commandText = "SELECT * FROM SuggestedTask WHERE Id = @Id";
            using (SqlConnection connection = CreateConnection())
            {
                var parameters = new
                {
                    Id = suggestedTaskId
                };

                var suggestedTask = await connection.QuerySingleOrDefaultAsync<SuggestedTask>(commandText, parameters);

                return suggestedTask;
            }
        }
        catch (Exception ex)
        {

            throw new($"Exception while trying to find the suggestedTask with the '{suggestedTaskId}'. The exception was: '{ex.Message}'", ex);


        }
    }

    public async Task<int> InsertSuggestedTaskAsync(SuggestedTask suggestedTask)
    {
        try
        {
            string commandText = "INSERT INTO SuggestedTask (Text, FKSuggestedLabelId) VALUES (@Text, @FKSuggestedLabelId); SELECT CAST(scope_identity() AS int)";
            using (SqlConnection connection = CreateConnection())
            {

                var parameters = new
                {
                    Text = suggestedTask.Text,
                    FKSuggestedLabelId = suggestedTask.FKSuggestedLabelId,
                };


                return suggestedTask.Id = await connection.QuerySingleAsync<int>(commandText, parameters);
            }
        }
        catch (Exception ex)
        {

            throw new($"Exception while trying to insert a SuggestedTask object. The exception was: '{ex.Message}'", ex);

        }
    }

    public async Task<bool> UpdateSuggestedTaskAsync(SuggestedTask suggestedTask)
    {
        try
        {
            string commandText = "UPDATE SuggestedTask SET " +
                "Text = @Text, " +
                "FKSuggestedLabelId = @FKSuggestedLabelId " +
                "WHERE Id = @Id";

            using (SqlConnection connection = CreateConnection())
            {
                var parameters = new
                {
                    Text = suggestedTask.Text,
                    FKSuggestedLabelId = suggestedTask.FKSuggestedLabelId,
                    Id = suggestedTask.Id
                };

                return await connection.ExecuteAsync(commandText, parameters) > 0;

            }
        }
        catch (Exception ex)
        {

            throw new Exception($"Exception while trying to update a SuggestedTask. The exception was: '{ex.Message}'", ex);

        }
    }
}
