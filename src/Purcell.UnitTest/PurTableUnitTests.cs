using System.Data;
using System.Globalization;
using System.Text;
using FluentAssertions;

namespace PurcellLibs.UnitTest;

/// <summary>
/// PurTable 单元测试类
/// 测试 PurTable 类的所有公共成员，确保功能正确性和边界条件处理
/// </summary>
public class PurTableUnitTests
{
    #region 构造函数测试

    [Fact]
    public void DefaultConstructor_ShouldInitializeWithExpectedDefaults()
    {
        // Act
        var sheet = new PurTable();

        // Assert
        sheet.SheetName.Should().BeEmpty();
        sheet.SheetIndex.Should().Be(0);
        sheet.HasHeader.Should().BeTrue();
        sheet.HeaderStart.Should().Be("A1");
        sheet.DataStart.Should().Be("");
        sheet.Columns.Should().NotBeNull().And.BeEmpty();
        sheet.MaxReadRows.Should().Be(-1);
        sheet.MaxWriteRows.Should().Be(-1);
        sheet.HeaderSpaceMode.Should().Be(WhiteSpaceMode.Trim);
        sheet.IgnoreParseError.Should().BeFalse();
        sheet.Culture.Should().Be(CultureInfo.InvariantCulture.Name);
        sheet.FileEncoding.Should().BeNull();
        sheet.CsvDelimiter.Should().Be(",");
        sheet.CsvEscape.Should().Be('"');
        sheet.SampleRows.Should().Be(5);
        sheet.AutoFilter.Should().BeTrue();
        sheet.Password.Should().BeNull();
        sheet.PresetStyle.Should().Be(PresetStyle.Default);
        sheet.TableStyle.Should().Be(PurStyle.Default);
        sheet.Records.Should().NotBeNull().And.BeEmpty();
    }

    [Theory]
    [InlineData("用户数据")]
    [InlineData("Sheet1")]
    [InlineData("数据分析")]
    [InlineData("A")]
    [InlineData("1234567890123456789012345678901")] // 31个字符，边界值
    public void ConstructorWithSheetName_ValidName_ShouldSetCorrectly(string sheetName)
    {
        // Act
        var sheet = new PurTable(sheetName);

        // Assert
        sheet.SheetName.Should().Be(sheetName);
        sheet.SheetIndex.Should().Be(0); // 默认值不变
    }

    [Fact]
    public void ConstructorWithSheetName_NullName_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action = () => new PurTable(null!);

        action.Should().Throw<ArgumentNullException>()
            .WithMessage("*工作表名称不能为 null*")
            .And.ParamName.Should().Be(nameof(PurTable.SheetName));
    }

    [Fact]
    public void ConstructorWithSheetName_ExceedsMaxLength_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var tooLongName = new string('A', 32); // 超过31个字符

        // Act & Assert
        var action = () => new PurTable(tooLongName);

        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*工作表名称不能超过31个字符*")
            .And.ParamName.Should().Be(nameof(PurTable.SheetName));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(255)] // 边界值
    public void ConstructorWithSheetIndex_ValidIndex_ShouldSetCorrectly(int sheetIndex)
    {
        // Act
        var sheet = new PurTable(sheetIndex);

        // Assert
        sheet.SheetIndex.Should().Be(sheetIndex);
        sheet.SheetName.Should().BeEmpty(); // 默认值不变
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(256)]
    [InlineData(-100)]
    [InlineData(1000)]
    public void ConstructorWithSheetIndex_InvalidIndex_ShouldThrowArgumentOutOfRangeException(int invalidIndex)
    {
        // Act & Assert
        var action = () => new PurTable(invalidIndex);

        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*工作表索引必须在 0-255 范围内*")
            .And.ParamName.Should().Be("SheetIndex");
    }

    [Fact]
    public void ConstructorWithColumnsAndIndex_ValidParameters_ShouldInitializeCorrectly()
    {
        // Arrange
        var columns = new List<PurColumn> { new("姓名"), new("年龄") };
        const int sheetIndex = 2;

        // Act
        var sheet = new PurTable(columns, sheetIndex);

        // Assert
        sheet.Columns.Should().HaveCount(2);
        sheet.SheetIndex.Should().Be(sheetIndex);
    }

    [Fact]
    public void ConstructorWithColumnsAndName_ValidParameters_ShouldInitializeCorrectly()
    {
        // Arrange
        var columns = new List<PurColumn> { new("姓名"), new("年龄") };
        const string sheetName = "员工数据";

        // Act
        var sheet = new PurTable(columns, sheetName);

        // Assert
        sheet.Columns.Should().HaveCount(2);
        sheet.SheetName.Should().Be(sheetName);
        sheet.SheetIndex.Should().Be(0);
    }

    [Fact]
    public void ConstructorWithColumnsAndName_NullColumns_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action = () => new PurTable((List<PurColumn>)null!, "测试工作表");

        action.Should().Throw<ArgumentNullException>()
            .WithMessage("*列配置集合不能为 null*")
            .And.ParamName.Should().Be(nameof(PurTable.Columns));
    }

    [Fact]
    public void ConstructorWithDataTableAndIndex_ValidParameters_ShouldInitializeCorrectly()
    {
        // Arrange
        var dataTable = CreateSampleDataTable();
        const int sheetIndex = 1;

        // Act
        var sheet = new PurTable(dataTable, sheetIndex);

        // Assert
        sheet.SheetIndex.Should().Be(sheetIndex);
        sheet.SheetName.Should().BeEmpty(); // 默认值
        sheet.Records.Should().NotBeEmpty();
        sheet.Records.Should().HaveCount(3); // 根据CreateSampleDataTable的数据行数
        
        // 验证数据内容正确性
        var recordsList = sheet.Records.ToList();
        recordsList[0]!["姓名"].Should().Be("张三");
        recordsList[0]!["年龄"].Should().Be(25);
        recordsList[0]!["城市"].Should().Be("北京");
    }

    [Fact]
    public void ConstructorWithDataTableAndIndex_DefaultIndex_ShouldUseZeroIndex()
    {
        // Arrange
        var dataTable = CreateSampleDataTable();

        // Act
        var sheet = new PurTable(dataTable);

        // Assert
        sheet.SheetIndex.Should().Be(0);
        sheet.Records.Should().NotBeEmpty();
        sheet.Records.Should().HaveCount(3);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(256)]
    [InlineData(1000)]
    public void ConstructorWithDataTableAndIndex_InvalidIndex_ShouldThrowArgumentOutOfRangeException(int invalidIndex)
    {
        // Arrange
        var dataTable = CreateSampleDataTable();

        // Act & Assert
        var action = () => new PurTable(dataTable, invalidIndex);

        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*工作表索引必须在 0-255 范围内*")
            .And.ParamName.Should().Be("SheetIndex");
    }

    [Fact]
    public void ConstructorWithDataTableAndIndex_NullDataTable_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action = () => new PurTable((DataTable)null!, 0);

        action.Should().Throw<ArgumentNullException>()
            .WithMessage("*dataTable*");
    }

    [Fact]
    public void ConstructorWithDataTableAndName_ValidParameters_ShouldInitializeCorrectly()
    {
        // Arrange
        var dataTable = CreateSampleDataTable();
        const string sheetName = "员工数据表";

        // Act
        var sheet = new PurTable(dataTable, sheetName);

        // Assert
        sheet.SheetName.Should().Be(sheetName);
        sheet.SheetIndex.Should().Be(0); // 默认值
        sheet.Records.Should().NotBeEmpty();
        sheet.Records.Should().HaveCount(3);
        
        // 验证数据内容正确性
        var recordsList = sheet.Records.ToList();
        recordsList[0]!["姓名"].Should().Be("张三");
        recordsList[1]!["姓名"].Should().Be("李四");
        recordsList[2]!["姓名"].Should().Be("王五");
    }

    [Fact]
    public void ConstructorWithDataTableAndName_NullDataTable_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action = () => new PurTable((DataTable)null!, "测试工作表");

        action.Should().Throw<ArgumentNullException>()
            .WithMessage("*dataTable*");
    }

    [Fact]
    public void ConstructorWithDataTableAndName_NullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dataTable = CreateSampleDataTable();

        // Act & Assert
        var action = () => new PurTable(dataTable, null!);

        action.Should().Throw<ArgumentNullException>()
            .WithMessage("*工作表名称不能为 null*")
            .And.ParamName.Should().Be(nameof(PurTable.SheetName));
    }

    [Fact]
    public void ConstructorWithDataTableAndName_ExceedsMaxLength_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var dataTable = CreateSampleDataTable();
        var tooLongName = new string('A', 32); // 超过31个字符

        // Act & Assert
        var action = () => new PurTable(dataTable, tooLongName);

        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*工作表名称不能超过31个字符*")
            .And.ParamName.Should().Be(nameof(PurTable.SheetName));
    }

    [Fact]
    public void ConstructorWithDataTableAndName_EmptyDataTable_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var emptyDataTable = new DataTable();

        // Act & Assert
        var action = () => new PurTable(emptyDataTable, "测试工作表");

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*无法从数据集合中获取任何列信息*");
    }

    [Fact]
    public void ConstructorWithDataTableAndIndex_EmptyDataTable_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var emptyDataTable = new DataTable();

        // Act & Assert
        var action = () => new PurTable(emptyDataTable, 0);

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*无法从数据集合中获取任何列信息*");
    }

    [Fact]
    public void ConstructorWithDataTable_ShouldCalculateColumnWidthsBasedOnContent()
    {
        // Arrange
        var dataTable = CreateSampleDataTable();

        // Act
        var sheet = new PurTable(dataTable, "测试");

        // Assert
        // 验证基本功能正常
        sheet.SheetName.Should().Be("测试");
        sheet.Records.Should().NotBeEmpty();
        sheet.Records.Should().HaveCount(3);
        
        // 验证数据内容正确性
        var recordsList = sheet.Records.ToList();
        recordsList[0]!["姓名"].Should().Be("张三");
        recordsList[0]!["年龄"].Should().Be(25);
        recordsList[0]!["城市"].Should().Be("北京");
    }

    #endregion

    #region 属性设置测试

    [Theory]
    [InlineData("测试工作表")]
    [InlineData("Test Sheet")]
    [InlineData("工作表123")]
    [InlineData("A")]
    public void SheetName_SetValidValue_ShouldUpdateProperty(string validName)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.SheetName = validName;

        // Assert
        sheet.SheetName.Should().Be(validName);
    }

    [Fact]
    public void SheetName_SetNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var sheet = new PurTable();

        // Act & Assert
        var action = () => sheet.SheetName = null!;

        action.Should().Throw<ArgumentNullException>()
            .WithMessage("*工作表名称不能为 null*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(128)]
    [InlineData(255)]
    public void SheetIndex_SetValidValue_ShouldUpdateProperty(int validIndex)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.SheetIndex = validIndex;

        // Assert
        sheet.SheetIndex.Should().Be(validIndex);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(256)]
    public void SheetIndex_SetInvalidValue_ShouldThrowArgumentOutOfRangeException(int invalidIndex)
    {
        // Arrange
        var sheet = new PurTable();

        // Act & Assert
        var action = () => sheet.SheetIndex = invalidIndex;

        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*工作表索引必须在 0-255 范围内*");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void HasHeader_SetValue_ShouldUpdateProperty(bool hasHeader)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.HasHeader = hasHeader;

        // Assert
        sheet.HasHeader.Should().Be(hasHeader);
    }

    [Theory]
    [InlineData("A1")]
    [InlineData("B2")]
    [InlineData("Z26")]
    [InlineData("AA1")]
    [InlineData("AB10")]
    public void HeaderStart_SetValidCellReference_ShouldUpdateProperty(string validCellRef)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.HeaderStart = validCellRef;

        // Assert
        sheet.HeaderStart.Should().Be(validCellRef);
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    [InlineData("1")]
    [InlineData("A0")]
    [InlineData("invalid")]
    [InlineData("1A")]
    [InlineData("@#$")]
    public void HeaderStart_SetInvalidCellReference_ShouldThrowArgumentException(string invalidCellRef)
    {
        // Arrange
        var sheet = new PurTable();

        // Act & Assert
        var action = () => sheet.HeaderStart = invalidCellRef;

        action.Should().Throw<ArgumentException>()
            .WithMessage("*无效的表头起始位置格式*");
    }

    [Theory]
    [InlineData("A2")]
    [InlineData("B3")]
    [InlineData("Z27")]
    [InlineData("AA2")]
    public void DataStart_SetValidCellReference_ShouldUpdateProperty(string validCellRef)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.DataStart = validCellRef;

        // Assert
        sheet.DataStart.Should().Be(validCellRef);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("1")]
    [InlineData("A0")]
    [InlineData("invalid")]
    public void DataStart_SetInvalidCellReference_ShouldThrowArgumentException(string invalidCellRef)
    {
        // Arrange
        var sheet = new PurTable();

        // Act & Assert
        var action = () => sheet.DataStart = invalidCellRef;

        action.Should().Throw<ArgumentException>()
            .WithMessage("*无效的数据起始位置格式*");
    }

    [Fact]
    public void Columns_SetValidList_ShouldUpdateProperty()
    {
        // Arrange
        var sheet = new PurTable();
        var columns = new List<PurColumn>
        {
            new("姓名"),
            new("年龄"),
            new("邮箱")
        };

        // Act
        sheet.Columns = columns;

        // Assert
        sheet.Columns.Should().BeEquivalentTo(columns);
        sheet.Columns.Should().HaveCount(3);
    }

    [Fact]
    public void Columns_SetNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var sheet = new PurTable();

        // Act & Assert
        var action = () => sheet.Columns = null!;

        action.Should().Throw<ArgumentNullException>()
            .WithMessage("*列配置集合不能为 null*");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1000)]
    [InlineData(int.MaxValue)]
    public void MaxReadRows_SetValue_ShouldUpdateProperty(int maxRows)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.MaxReadRows = maxRows;

        // Assert
        sheet.MaxReadRows.Should().Be(maxRows);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1000)]
    public void MaxWriteRows_SetValue_ShouldUpdateProperty(int maxRows)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.MaxWriteRows = maxRows;

        // Assert
        sheet.MaxWriteRows.Should().Be(maxRows);
    }

    [Theory]
    [InlineData(WhiteSpaceMode.Preserve)]
    [InlineData(WhiteSpaceMode.Trim)]
    [InlineData(WhiteSpaceMode.RemoveAll)]
    public void HeaderSpaceMode_SetValue_ShouldUpdateProperty(WhiteSpaceMode mode)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.HeaderSpaceMode = mode;

        // Assert
        sheet.HeaderSpaceMode.Should().Be(mode);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IgnoreParseError_SetValue_ShouldUpdateProperty(bool ignoreError)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.IgnoreParseError = ignoreError;

        // Assert
        sheet.IgnoreParseError.Should().Be(ignoreError);
    }

    [Theory]
    [InlineData("zh-CN")]
    [InlineData("en-US")]
    [InlineData("ja-JP")]
    [InlineData("fr-FR")]
    [InlineData("de-DE")]
    public void Culture_SetValidValue_ShouldUpdateProperty(string culture)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.Culture = culture;

        // Assert
        sheet.Culture.Should().Be(culture);
    }

    [Theory]
    [InlineData("invalid-culture")]
    [InlineData("zhCN")]
    [InlineData("xxx")]
    public void Culture_SetInvalidValue_ShouldThrowArgumentException(string invalidCulture)
    {
        // Arrange
        var sheet = new PurTable();

        // Act & Assert
        var action = () => sheet.Culture = invalidCulture;

        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("utf-8", "Unicode (UTF-8)")]
    [InlineData("utf-16", "Unicode")]
    [InlineData("ascii", "US-ASCII")]
    [InlineData(null, null)]
    public void FileEncoding_SetValidValue_ShouldUpdateProperty(string? encoding, string? expectedDisplay)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.FileEncoding = encoding;

        // Assert
        sheet.FileEncoding.Should().Be(expectedDisplay);
    }

    [Theory]
    [InlineData("invalid-encoding")]
    [InlineData("not-exist")]
    [InlineData("gb23010")] // 在某些系统上不支持
    public void FileEncoding_SetInvalidValue_ShouldThrowArgumentException(string invalidEncoding)
    {
        // Arrange
        var sheet = new PurTable();

        // Act & Assert
        var action = () => sheet.FileEncoding = invalidEncoding;

        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(",")]
    [InlineData(";")]
    [InlineData("\t")]
    [InlineData("|")]
    [InlineData("||")]
    public void CsvDelimiter_SetValidValue_ShouldUpdateProperty(string delimiter)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.CsvDelimiter = delimiter;

        // Assert
        sheet.CsvDelimiter.Should().Be(delimiter);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CsvDelimiter_SetNullOrEmpty_ShouldThrowArgumentNullException(string? invalidDelimiter)
    {
        // Arrange
        var sheet = new PurTable();

        // Act & Assert
        var action = () => sheet.CsvDelimiter = invalidDelimiter!;

        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData('"')]
    [InlineData('\'')]
    [InlineData('`')]
    [InlineData('~')]
    public void CsvEscape_SetValidValue_ShouldUpdateProperty(char escape)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.CsvEscape = escape;

        // Assert
        sheet.CsvEscape.Should().Be(escape);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(100)]
    public void SampleRows_SetValidValue_ShouldUpdateProperty(int sampleRows)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.SampleRows = sampleRows;

        // Assert
        sheet.SampleRows.Should().Be(sampleRows);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void SampleRows_SetNegativeValue_ShouldThrowArgumentOutOfRangeException(int negativeSampleRows)
    {
        // Arrange
        var sheet = new PurTable();

        // Act & Assert
        var action = () => sheet.SampleRows = negativeSampleRows;

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AutoFilter_SetValue_ShouldUpdateProperty(bool autoFilter)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.AutoFilter = autoFilter;

        // Assert
        sheet.AutoFilter.Should().Be(autoFilter);
    }

    [Theory]
    [InlineData("password123")]
    [InlineData("")]
    [InlineData("复杂密码@#$")]
    [InlineData(null)]
    public void Password_SetValue_ShouldUpdateProperty(string? password)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.Password = password;

        // Assert
        sheet.Password.Should().Be(password);
    }

    [Theory]
    [InlineData(PresetStyle.Default)]
    [InlineData(PresetStyle.BrightFresh)]
    [InlineData(PresetStyle.ElegantMonochrome)]
    [InlineData(PresetStyle.EarthTones)]
    public void PresetStyle_SetValue_ShouldUpdateProperty(PresetStyle presetStyle)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.PresetStyle = presetStyle;

        // Assert
        sheet.PresetStyle.Should().Be(presetStyle);
    }

    [Fact]
    public void TableStyle_SetValue_ShouldUpdateProperty()
    {
        // Arrange
        var sheet = new PurTable();
        var style = PurStyle.BrightFresh;

        // Act
        sheet.TableStyle = style;

        // Assert
        sheet.TableStyle.Should().Be(style);
    }

    #endregion

    #region 方法测试

    [Theory]
    [InlineData("A1")]
    [InlineData("B3")]
    [InlineData("Z26")]
    [InlineData("AA1")]
    public void GetHeaderStart_ShouldReturnCorrectCellLocator(string headerStart)
    {
        // Arrange
        var sheet = new PurTable { HeaderStart = headerStart };

        // Act
        var result = sheet.GetHeaderStart();

        // Assert
        result.Should().Be(new CellLocator(headerStart));
    }

    [Fact]
    public void GetDataStart_WithDefaultDataStart_ShouldReturnNextRowFromHeaderStart()
    {
        // Arrange
        var sheet = new PurTable { HeaderStart = "B3" };

        // Act
        var result = sheet.GetDataStart();

        // Assert
        result.Should().Be(new CellLocator("B4"));
    }

    [Fact]
    public void GetDataStart_WithCustomDataStart_ShouldReturnCustomValue()
    {
        // Arrange
        var sheet = new PurTable
        {
            HeaderStart = "B3",
            DataStart = "B5"
        };

        // Act
        var result = sheet.GetDataStart();

        // Assert
        result.Should().Be(new CellLocator("B5"));
    }

    [Fact]
    public void GetCulture_WithDefaultCulture_ShouldReturnInvariantCulture()
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        var result = sheet.GetCulture();

        // Assert
        result.Should().Be(CultureInfo.InvariantCulture);
    }

    [Theory]
    [InlineData("zh-CN")]
    [InlineData("en-US")]
    [InlineData("ja-JP")]
    public void GetCulture_WithCustomCulture_ShouldReturnCorrectCulture(string cultureName)
    {
        // Arrange
        var sheet = new PurTable { Culture = cultureName };

        // Act
        var result = sheet.GetCulture();

        // Assert
        result.Should().Be(new CultureInfo(cultureName));
    }

    [Fact]
    public void GetFileEncoding_WithNullEncoding_ShouldReturnNull()
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        var result = sheet.GetFileEncoding();

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("utf-8")]
    [InlineData("gb2312")]
    [InlineData("utf-16")]
    public void GetFileEncoding_WithValidEncoding_ShouldReturnCorrectEncoding(string encodingName)
    {
        // Arrange
        var sheet = new PurTable { FileEncoding = encodingName };
        var expectedEncoding = Encoding.GetEncoding(encodingName);

        // Act
        var result = sheet.GetFileEncoding();

        // Assert
        result.Should().Be(expectedEncoding);
    }

    [Fact]
    public void GetActualStyle_WithDefaultStyles_ShouldReturnDefaultStyle()
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        var result = sheet.GetActualStyle();

        // Assert
        result.Should().Be(PurStyle.Default);
    }

    [Fact]
    public void GetActualStyle_WithCustomTableStyle_ShouldReturnTableStyle()
    {
        // Arrange
        var sheet = new PurTable { TableStyle = PurStyle.BrightFresh };

        // Act
        var result = sheet.GetActualStyle();

        // Assert
        result.Should().Be(PurStyle.BrightFresh);
    }

    [Fact]
    public void GetActualStyle_WithPresetStyle_ShouldReturnPresetStyleMapping()
    {
        // Arrange
        var sheet = new PurTable { PresetStyle = PresetStyle.BrightFresh };

        // Act
        var result = sheet.GetActualStyle();

        // Assert
        result.Should().Be(PurStyle.BrightFresh);
    }

    [Fact]
    public void GetActualStyle_TableStyleTakesPrecedenceOverPresetStyle()
    {
        // Arrange
        var sheet = new PurTable
        {
            TableStyle = PurStyle.ElegantMonochrome,
            PresetStyle = PresetStyle.BrightFresh
        };

        // Act
        var result = sheet.GetActualStyle();

        // Assert
        result.Should().Be(PurStyle.ElegantMonochrome);
    }

    #endregion

    #region 流畅API测试

    [Fact]
    public void WithName_ShouldSetNameAndReturnSelf()
    {
        // Arrange
        var sheet = new PurTable();
        const string name = "测试工作表";

        // Act
        var result = sheet.WithName(name);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.SheetName.Should().Be(name);
    }

    [Fact]
    public void WithIndex_ShouldSetIndexAndReturnSelf()
    {
        // Arrange
        var sheet = new PurTable();
        const int index = 3;

        // Act
        var result = sheet.WithIndex(index);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.SheetIndex.Should().Be(index);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void WithHasHeader_ShouldSetHasHeaderAndReturnSelf(bool hasHeader)
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        var result = sheet.WithHasHeader(hasHeader);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.HasHeader.Should().Be(hasHeader);
    }

    [Fact]
    public void WithoutHeader_ShouldSetHasHeaderToFalseAndReturnSelf()
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        var result = sheet.WithoutHeader();

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.HasHeader.Should().BeFalse();
    }

    [Fact]
    public void WithHeaderStart_String_ShouldSetHeaderStartAndReturnSelf()
    {
        // Arrange
        var sheet = new PurTable();
        const string headerStart = "B3";

        // Act
        var result = sheet.WithHeaderStart(headerStart);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.HeaderStart.Should().Be(headerStart);
    }

    [Fact]
    public void WithHeaderStart_CellLocator_ShouldSetHeaderStartAndReturnSelf()
    {
        // Arrange
        var sheet = new PurTable();
        var headerStart = new CellLocator("B3");

        // Act
        var result = sheet.WithHeaderStart(headerStart);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.HeaderStart.Should().Be("B3");
    }

    [Fact]
    public void WithHeaderStart_RowColumn_ShouldSetHeaderStartAndReturnSelf()
    {
        // Arrange
        var sheet = new PurTable();
        const int row = 2; // 0-based, 第3行
        const int column = 1; // 0-based, B列

        // Act
        var result = sheet.WithHeaderStart(row, column);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.HeaderStart.Should().Be("B3");
    }

    [Fact]
    public void WithDataStart_String_ShouldSetDataStartAndReturnSelf()
    {
        // Arrange
        var sheet = new PurTable();
        const string dataStart = "B4";

        // Act
        var result = sheet.WithDataStart(dataStart);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.DataStart.Should().Be(dataStart);
    }

    [Fact]
    public void WithStart_TwoStrings_ShouldSetBothStartsAndReturnSelf()
    {
        // Arrange
        var sheet = new PurTable();
        const string headerStart = "B3";
        const string dataStart = "B4";

        // Act
        var result = sheet.WithStart(headerStart, dataStart);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.HeaderStart.Should().Be(headerStart);
        sheet.DataStart.Should().Be(dataStart);
    }

    [Fact]
    public void WithStart_TwoRows_ShouldSetBothStartsAndReturnSelf()
    {
        // Arrange
        var sheet = new PurTable();
        const int headerRow = 1; // 第2行
        const int dataRow = 2; // 第3行

        // Act
        var result = sheet.WithStart(headerRow, dataRow);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.HeaderStart.Should().Be("A2");
        sheet.DataStart.Should().Be("A3");
    }

    [Fact]
    public void WithColumns_List_ShouldSetColumnsAndReturnSelf()
    {
        // Arrange
        var sheet = new PurTable();
        var columns = new List<PurColumn> { new("姓名"), new("年龄") };

        // Act
        var result = sheet.WithColumns(columns);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.Columns.Should().BeEquivalentTo(columns);
    }

    [Fact]
    public void WithColumns_Array_ShouldSetColumnsAndReturnSelf()
    {
        // Arrange
        var sheet = new PurTable();
        var columns = new[] { new PurColumn("姓名"), new PurColumn("年龄") };

        // Act
        var result = sheet.WithColumns(columns);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.Columns.Should().BeEquivalentTo(columns);
    }

    [Fact]
    public void AddColumn_ShouldAddColumnAndReturnSelf()
    {
        // Arrange
        var sheet = new PurTable();
        var column = new PurColumn("姓名");

        // Act
        var result = sheet.AddColumn(column);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.Columns.Should().Contain(column);
        sheet.Columns.Should().HaveCount(1);
    }

    [Fact]
    public void AddColumns_List_ShouldAddColumnsAndReturnSelf()
    {
        // Arrange
        var sheet = new PurTable();
        var existingColumn = new PurColumn("ID");
        sheet.AddColumn(existingColumn);

        var newColumns = new List<PurColumn> { new("姓名"), new("年龄") };

        // Act
        var result = sheet.AddColumns(newColumns);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.Columns.Should().HaveCount(3);
        sheet.Columns.Should().Contain(existingColumn);
        sheet.Columns.Should().Contain(newColumns[0]);
        sheet.Columns.Should().Contain(newColumns[1]);
    }

    [Fact]
    public void FluentChaining_ShouldWorkCorrectly()
    {
        // Act
        var sheet = new PurTable()
            .WithName("用户数据")
            .WithIndex(2)
            .WithHasHeader(true)
            .WithHeaderStart("B2")
            .WithDataStart("B3")
            .WithMaxReadRows(100)
            .WithMaxWriteRows(200)
            .WithCulture("zh-CN")
            .WithCsvDelimiter(";")
            .WithSampleRows(10)
            .WithAutoFilter(false)
            .WithPassword("secret123");

        // Assert
        sheet.SheetName.Should().Be("用户数据");
        sheet.SheetIndex.Should().Be(2);
        sheet.HasHeader.Should().BeTrue();
        sheet.HeaderStart.Should().Be("B2");
        sheet.DataStart.Should().Be("B3");
        sheet.MaxReadRows.Should().Be(100);
        sheet.MaxWriteRows.Should().Be(200);
        sheet.Culture.Should().Be("zh-CN");
        sheet.CsvDelimiter.Should().Be(";");
        sheet.SampleRows.Should().Be(10);
        sheet.AutoFilter.Should().BeFalse();
        sheet.Password.Should().Be("secret123");
    }

    #endregion

    #region 静态工厂方法测试

    [Fact]
    public void New_ShouldReturnNewInstanceWithDefaults()
    {
        // Act
        var sheet = PurTable.New();

        // Assert
        sheet.Should().NotBeNull();
        sheet.Should().BeOfType<PurTable>();
        sheet.SheetName.Should().BeEmpty();
    }

    [Fact]
    public void From_SheetName_ShouldReturnInstanceWithCorrectName()
    {
        // Arrange
        const string sheetName = "测试工作表";

        // Act
        var sheet = PurTable.From(sheetName);

        // Assert
        sheet.SheetName.Should().Be(sheetName);
    }

    [Fact]
    public void From_SheetIndex_ShouldReturnInstanceWithCorrectIndex()
    {
        // Arrange
        const int sheetIndex = 3;

        // Act
        var sheet = PurTable.From(sheetIndex);

        // Assert
        sheet.SheetIndex.Should().Be(sheetIndex);
    }

    [Fact]
    public void FromName_ShouldReturnInstanceWithCorrectName()
    {
        // Arrange
        const string sheetName = "员工数据";

        // Act
        var sheet = PurTable.FromName(sheetName);

        // Assert
        sheet.SheetName.Should().Be(sheetName);
    }

    [Fact]
    public void FromIndex_ShouldReturnInstanceWithCorrectIndex()
    {
        // Arrange
        const int sheetIndex = 5;

        // Act
        var sheet = PurTable.FromIndex(sheetIndex);

        // Assert
        sheet.SheetIndex.Should().Be(sheetIndex);
    }

    [Fact]
    public void FromRecords_DataTableWithIndex_ShouldReturnInstanceWithCorrectConfiguration()
    {
        // Arrange
        var dataTable = CreateSampleDataTable();
        const int sheetIndex = 2;

        // Act
        var sheet = PurTable.FromRecords(dataTable, sheetIndex);

        // Assert
        sheet.SheetIndex.Should().Be(sheetIndex);
        sheet.SheetName.Should().BeEmpty(); // 默认值
        sheet.Records.Should().NotBeEmpty();
        sheet.Records.Should().HaveCount(3);
        
        // 验证数据内容正确性
        var recordsList = sheet.Records.ToList();
        recordsList[0]!["姓名"].Should().Be("张三");
        recordsList[1]!["年龄"].Should().Be(30);
        recordsList[2]!["城市"].Should().Be("广州");
    }

    [Fact]
    public void FromRecords_DataTableWithIndex_DefaultIndex_ShouldUseZeroIndex()
    {
        // Arrange
        var dataTable = CreateSampleDataTable();

        // Act
        var sheet = PurTable.FromRecords(dataTable);

        // Assert
        sheet.SheetIndex.Should().Be(0);
        sheet.Records.Should().NotBeEmpty();
        sheet.Records.Should().HaveCount(3);
    }

    [Fact]
    public void FromRecords_DataTableWithName_ShouldReturnInstanceWithCorrectConfiguration()
    {
        // Arrange
        var dataTable = CreateSampleDataTable();
        const string sheetName = "数据报表";

        // Act
        var sheet = PurTable.FromRecords(dataTable, sheetName);

        // Assert
        sheet.SheetName.Should().Be(sheetName);
        sheet.SheetIndex.Should().Be(0); // 默认值
        sheet.Records.Should().NotBeEmpty();
        sheet.Records.Should().HaveCount(3);
        
        // 验证数据内容正确性
        var recordsList = sheet.Records.ToList();
        recordsList[0]!["姓名"].Should().Be("张三");
        recordsList[1]!["年龄"].Should().Be(30);
        recordsList[2]!["城市"].Should().Be("广州");
    }

    [Fact]
    public void FromRecords_DataTableWithNullDataTable_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action1 = () => PurTable.FromRecords((DataTable)null!, 0);
        var action2 = () => PurTable.FromRecords((DataTable)null!, "测试表");

        action1.Should().Throw<ArgumentNullException>()
            .WithMessage("*dataTable*");
        action2.Should().Throw<ArgumentNullException>()
            .WithMessage("*dataTable*");
    }

    [Fact]
    public void FromRecords_DataTableWithInvalidIndex_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var dataTable = CreateSampleDataTable();

        // Act & Assert
        var action = () => PurTable.FromRecords(dataTable, -1);

        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*工作表索引必须在 0-255 范围内*");
    }

    [Fact]
    public void FromRecords_DataTableWithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dataTable = CreateSampleDataTable();

        // Act & Assert
        var action = () => PurTable.FromRecords(dataTable, null!);

        action.Should().Throw<ArgumentNullException>()
            .WithMessage("*工作表名称不能为 null*");
    }

    [Fact]
    public void FromRecords_DataTableWithEmptyDataTable_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var emptyDataTable = new DataTable();

        // Act & Assert
        var action1 = () => PurTable.FromRecords(emptyDataTable, 0);
        var action2 = () => PurTable.FromRecords(emptyDataTable, "测试表");

        action1.Should().Throw<InvalidOperationException>()
            .WithMessage("*无法从数据集合中获取任何列信息*");
        action2.Should().Throw<InvalidOperationException>()
            .WithMessage("*无法从数据集合中获取任何列信息*");
    }

    #endregion

    #region WithRecords 测试

    [Fact]
    public void WithRecords_DataTable_ShouldSetRecordsAndGenerateColumns()
    {
        // Arrange
        var sheet = new PurTable();
        var dataTable = CreateSampleDataTable();

        // Act
        var result = sheet.WithRecords(dataTable);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.Records.Should().NotBeEmpty();
        sheet.Records.Should().HaveCount(3); // 根据CreateSampleDataTable的数据行数
        
        // 验证数据内容正确性
        var recordsList = sheet.Records.ToList();
        recordsList[0]!["姓名"].Should().Be("张三");
        recordsList[1]!["姓名"].Should().Be("李四");
        recordsList[2]!["姓名"].Should().Be("王五");
    }

    [Fact]
    public void WithRecords_NullDataTable_ShouldThrowArgumentNullException()
    {
        // Arrange
        var sheet = new PurTable();

        // Act & Assert
        var action = () => sheet.WithRecords((DataTable)null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WithRecords_GenericCollection_ShouldSetRecordsAndGenerateColumns()
    {
        // Arrange
        var sheet = new PurTable();
        var records = new List<object>
        {
            new { 姓名 = "张三", 年龄 = 25, 邮箱 = "zhangsan@test.com" },
            new { 姓名 = "李四", 年龄 = 30, 邮箱 = "lisi@test.com" }
        };

        // Act
        var result = sheet.WithRecords(records);

        // Assert
        result.Should().BeSameAs(sheet);
        sheet.Records.Should().NotBeEmpty();
        sheet.Records.Should().HaveCount(2);
    }

    [Fact]
    public void WithRecords_NullCollection_ShouldThrowArgumentNullException()
    {
        // Arrange
        var sheet = new PurTable();

        // Act & Assert
        var action = () => sheet.WithRecords<object>(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WithRecords_EmptyCollection_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var sheet = new PurTable();
        var emptyRecords = new List<object>();

        // Act & Assert
        var action = () => sheet.WithRecords(emptyRecords);

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*无法从数据集合中获取任何列信息*");
    }

    #endregion

    #region 边界条件和异常情况测试

    [Fact]
    public void SheetName_MaxLength_ShouldAccept31Characters()
    {
        // Arrange
        var sheet = new PurTable();
        var maxLengthName = new string('A', 31);

        // Act
        sheet.SheetName = maxLengthName;

        // Assert
        sheet.SheetName.Should().Be(maxLengthName);
        sheet.SheetName.Should().HaveLength(31);
    }

    [Fact]
    public void SheetIndex_BoundaryValues_ShouldWorkCorrectly()
    {
        // Arrange
        var sheet = new PurTable();

        // Act & Assert
        sheet.SheetIndex = 0; // 最小值
        sheet.SheetIndex.Should().Be(0);

        sheet.SheetIndex = 255; // 最大值
        sheet.SheetIndex.Should().Be(255);
    }

    [Fact]
    public void SampleRows_ZeroValue_ShouldBeAllowed()
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.SampleRows = 0;

        // Assert
        sheet.SampleRows.Should().Be(0);
    }

    [Fact]
    public void MultiplePropertyChanges_ShouldAllBeReflected()
    {
        // Arrange
        var sheet = new PurTable();

        // Act
        sheet.SheetName = "测试表";
        sheet.SheetIndex = 1;
        sheet.HasHeader = false;
        sheet.MaxReadRows = 50;
        sheet.IgnoreParseError = true;

        // Assert  
        sheet.SheetName.Should().Be("测试表");
        sheet.SheetIndex.Should().Be(1);
        sheet.HasHeader.Should().BeFalse();
        sheet.MaxReadRows.Should().Be(50);
        sheet.IgnoreParseError.Should().BeTrue();
    }

    #endregion

    #region 测试辅助方法

    private static DataTable CreateSampleDataTable()
    {
        var dataTable = new DataTable();
        dataTable.Columns.Add("姓名", typeof(string));
        dataTable.Columns.Add("年龄", typeof(int));
        dataTable.Columns.Add("城市", typeof(string));

        var row1 = dataTable.NewRow();
        row1["姓名"] = "张三";
        row1["年龄"] = 25;
        row1["城市"] = "北京";
        dataTable.Rows.Add(row1);

        var row2 = dataTable.NewRow();
        row2["姓名"] = "李四";
        row2["年龄"] = 30;
        row2["城市"] = "上海";
        dataTable.Rows.Add(row2);

        var row3 = dataTable.NewRow();
        row3["姓名"] = "王五";
        row3["年龄"] = 28;
        row3["城市"] = "广州";
        dataTable.Rows.Add(row3);

        return dataTable;
    }

    #endregion
}