namespace TasksAPI.Data;

public static class SqlRepositoryFactory
{
    public static T CreateRepo<T>(string connectionstring) where T : class
    {
        switch (typeof(T).Name)
        {
            case "IPinnedTaskRepository":
                return new PinnedTaskRepository(connectionstring) as T;
            case "ISuggestedTaskRepository":
                return new SuggestedTaskRepository(connectionstring) as T;
            case "ISuggestedLabelRepository":
                return new ISuggestedLabelRepository(connectionstring) as T;

        }
        throw new ArgumentException($"Unknown type {typeof(T).FullName}");
    }
}
