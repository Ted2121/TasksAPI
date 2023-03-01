using Dapper;
using System.Data.SqlClient;
using TasksAPI.Models;

namespace TasksAPI.Data;

public class SuggestedLabelRepository : BaseDataAccess, ISuggestedLabelRepository
{
    public SuggestedLabelRepository(string connectionstring) : base(connectionstring)
    {
    }

    public async Task<bool> DeleteSuggestedLabelAsync(int suggestedLabelId)
    {
        string commandText = "DELETE FROM SuggestedLabel WHERE Id = @Id";
        try
        {
            using (SqlConnection connection = CreateConnection())
            {
                var parameters = new
                {
                    Id = suggestedLabelId
                };
                return await connection.ExecuteAsync(commandText, parameters) > 0;
            }

        }
        catch (Exception ex)
        {
            throw new Exception($"Exception while trying to delete a row from SuggestedLabel table. The exception was: '{ex.Message}'", ex);
        }
    }

    public async Task<IEnumerable<SuggestedLabel>> GetAllSuggestedLabelsAsync()
    {
        try
        {
            string commandText = "SELECT * FROM SuggestedLabel";
            using (SqlConnection connection = CreateConnection())
            {
                var suggestedLabels = await connection.QueryAsync<SuggestedLabel>(commandText);

                return suggestedLabels;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception while trying to read all rows from the SuggestedLabel table. The exception was: '{ex.Message}'", ex);
        }
    }

    public async Task<SuggestedLabel> GetSuggestedLabelByIdAsync(int suggestedLabelId)
    {
        try
        {
            string commandText = "SELECT * FROM SuggestedLabel WHERE Id = @Id";
            using (SqlConnection connection = CreateConnection())
            {
                var parameters = new
                {
                    Id = suggestedLabelId
                };

                var suggestedLabel = await connection.QuerySingleOrDefaultAsync<SuggestedLabel>(commandText, parameters);

                return suggestedLabel;
            }
        }
        catch (Exception ex)
        {

            throw new($"Exception while trying to find the suggestedLabel with the '{suggestedLabelId}'. The exception was: '{ex.Message}'", ex);


        }
    }

    public async Task<int> InsertSuggestedLabelAsync(SuggestedLabel suggestedLabel)
    {
        try
        {
            string commandText = "INSERT INTO SuggestedLabel (Name) VALUES (@Name); SELECT CAST(scope_identity() AS int)";
            using (SqlConnection connection = CreateConnection())
            {

                var parameters = new
                {
                    Name = suggestedLabel.Name,
                };


                return suggestedLabel.Id = await connection.QuerySingleAsync<int>(commandText, parameters);
            }
        }
        catch (Exception ex)
        {

            throw new($"Exception while trying to insert a SuggestedLabel object. The exception was: '{ex.Message}'", ex);

        }
    }

    public async Task<bool> UpdateSuggestedLabelAsync(SuggestedLabel suggestedLabel)
    {
        try
        {
            string commandText = "UPDATE SuggestedLabel " +
                "Name = @Name, " +
                "WHERE Id = @Id";

            using (SqlConnection connection = CreateConnection())
            {
                var parameters = new
                {
                    Name = suggestedLabel.Name,
                    Id = suggestedLabel.Id
                };

                return await connection.ExecuteAsync(commandText, parameters) > 0;

            }
        }
        catch (Exception ex)
        {

            throw new Exception($"Exception while trying to update a SuggestedLabel. The exception was: '{ex.Message}'", ex);

        }
    }
}
