namespace PurcellLibs.Utils;

/// <summary>
/// 列操作工具类，提供列名匹配、列属性处理等功能。
/// </summary>
public static class ColumnUtils
{
    /// <summary>
    /// 根据指定的匹配策略，将自动列与动态列列表进行匹配。
    /// </summary>
    /// <param name="colIndex">要匹配的列索引。</param>
    /// <param name="propName">要匹配的属性名称。</param>
    /// <param name="dynamicColumns">用于匹配的动态列列表。</param>
    /// <param name="whiteSpaceMode">空白字符处理模式。</param>
    /// <returns>匹配成功的 <see cref="PurColumn"/> 列表；如果没有匹配项则返回空列表。</returns>
    public static List<PurColumn> MatchColumns(int colIndex, string propName, List<PurColumn> dynamicColumns,
        WhiteSpaceMode whiteSpaceMode)
    {
        List<PurColumn> matchedColumns = [];

        // 在属性名称匹配时，根据 .NET 的规则移除所有空格。
        // string procPropName = propName.ProcessWhiteSpace(whiteSpaceMode);

        foreach (PurColumn dyc in
                 dynamicColumns.Where(dyc => dyc.IgnoreInQuery == false && !string.IsNullOrWhiteSpace(dyc.PropertyName)))
        {
            if (dyc.Index == colIndex)
            {
                matchedColumns.Add(dyc);
                continue;
            }

            // 处理忽略大小写的情况。
            StringComparison strCpn = dyc.MatchStrategy.HasFlag(MatchStrategy.IgnoreCase)
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            List<string> dycNames = dyc.Names
                .Select(c => c.ProcessWhiteSpace(whiteSpaceMode))
                .ToList()
                .Concat(dyc.Names).Distinct()
                .ToList();

            // 根据不同策略查找匹配的列。
            if (dyc.MatchStrategy.HasFlag(MatchStrategy.Contains))
            {
                if (propName.IndexOf(dyc.PropertyName, strCpn) >= 0 ||
                    // procPropName.IndexOf(dyc.PropertyName, strCpn) >= 0 ||
                    dycNames.Any(c => propName.IndexOf(c, strCpn) >= 0)
                    //dycNames.Any(c => procPropName.IndexOf(c, strCpn) >= 0)
                   )
                {
                    matchedColumns.Add(dyc);
                }
            }
            else if (dyc.MatchStrategy.HasFlag(MatchStrategy.Prefix))
            {
                if (propName.StartsWith(dyc.PropertyName, strCpn) ||
                    //procPropName.StartsWith(dyc.PropertyName, strCpn) ||
                    dycNames.Any(c => propName.StartsWith(c, strCpn))
                    //dycNames.Any(c => procPropName.StartsWith(c, strCpn))
                   )
                {
                    matchedColumns.Add(dyc);
                }
            }
            else if (dyc.MatchStrategy.HasFlag(MatchStrategy.Suffix))
            {
                if (propName.EndsWith(dyc.PropertyName, strCpn) ||
                    //procPropName.EndsWith(dyc.PropertyName, strCpn) ||
                    dycNames.Any(c => propName.EndsWith(c, strCpn))
                    //dycNames.Any(c => procPropName.EndsWith(c, strCpn))
                   )
                {
                    matchedColumns.Add(dyc);
                }
            }
            else if (dyc.MatchStrategy.HasFlag(MatchStrategy.Regex))
            {
                RegexOptions regexOptions = dyc.MatchStrategy.HasFlag(MatchStrategy.IgnoreCase)
                    ? RegexOptions.IgnoreCase
                    : RegexOptions.None;
                if (propName.Equals(dyc.PropertyName, strCpn) ||
                    //procPropName.Equals(dyc.PropertyName, strCpn) ||
                    dyc.Names.Any(c => Regex.IsMatch(propName, c, regexOptions)))
                    matchedColumns.Add(dyc);
            }
            else
            {
                StringComparer strCpr = dyc.MatchStrategy.HasFlag(MatchStrategy.IgnoreCase)
                    ? StringComparer.OrdinalIgnoreCase
                    : StringComparer.Ordinal;
                if (propName.Equals(dyc.PropertyName, strCpn) ||
                    //procPropName.Equals(dyc.PropertyName, strCpn) ||
                    dycNames.Contains(propName, strCpr)
                    //dycNames.Contains(procPropName, strCpr)
                   )
                {
                    matchedColumns.Add(dyc);
                }
            }
        }

        return matchedColumns;
    }
}