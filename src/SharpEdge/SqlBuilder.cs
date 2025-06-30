namespace SharpEdge;

public class SqlBuilder
{
    public static SqlQuery Sql(FormattableString query)
    {
        var sql = query.Format;
        var parameters = new Dictionary<string, object?>();

        for (int i = 0; i < query.ArgumentCount; i++)
        {
            var paramName = $"@p{i}";
            sql = sql.Replace($"{{{i}}}", paramName);
            parameters[paramName] = query.GetArgument(i);
        }

        return new SqlQuery(sql, parameters);
    }

    public void Test1()
    {
        var userId = 123;
        var status = "active";
        FormattableString sql = $"""
                                 SELECT * FROM users 
                                 WHERE id = {userId} 
                                 AND status = {status}
                                 """;

        // 使用
        var query = SqlBuilder.Sql(sql);
        
        // query.Text: "SELECT * FROM users WHERE name = @p0 AND age > @p1"
        // query.Parameters: {"@p0": "Alice", "@p1": 25}
    }
}

public record SqlQuery(string Text, Dictionary<string, object?> Parameters);