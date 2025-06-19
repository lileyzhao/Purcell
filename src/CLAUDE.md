# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Purcell is a high-performance Excel/CSV library for reading and writing tabular data with strong typing. It supports `.xls`, `.xlsx`, `.xlsb`, and `.csv` formats, designed for .NET with a focus on performance and ease of use.

## Core Architecture

### Main Components

- **Purcell (Static API)**: Main entry point providing `Query<T>()` and `Export<T>()` methods with sync/async variants
- **PurTable**: Configuration class for table operations (headers, columns, data ranges, styles)
- **PurColumn**: Column configuration with mapping, formatting, and validation
- **Providers**: Abstract layer separating implementation from API
  - `IPurQuerier`/`PurQuerier`: Data reading operations
  - `IPurExporter`/`PurExporter`: Data writing operations
- **TableReader/TableWriter**: Low-level file format handlers
  - `SylvanExcelTableReader`: Excel file reading
  - `CsvHelperTableReader`: CSV file reading
  - `LargeXlsxTableWriter`: Large Excel file writing
  - `CsvHelperTableWriter`: CSV file writing

### Key Design Patterns

- **Factory Pattern**: `Purcell.CreateQuerier()` and `Purcell.CreateExporter()` methods
- **Provider Pattern**: Abstraction layer for different file formats
- **Fluent API**: `PurTable.From().WithColumns().WithStyle()` chaining
- **Attribute-based Configuration**: `[PurTable]`, `[PurColumn]` attributes for metadata

## Development Commands

### Build
```bash
dotnet build src/Purcell.sln
```

### Test
```bash
# Run all tests
dotnet test src/Purcell.UnitTest/

# Run specific test file
dotnet test src/Purcell.UnitTest/ --filter "ClassName~Tests_Query_Generic"
```

### Benchmarks
```bash
dotnet run --project src/Purcell.Benchmarks/ -c Release
```

### Console App (Examples)
```bash
dotnet run --project src/Purcell.ConsoleApp/
```

## Project Structure

- `Purcell/`: Main library code
  - `Attributes/`: PurTable, PurColumn attribute definitions
  - `Common/`: Enums and constants (ExportType, QueryType, etc.)
  - `Converters/`: Type converters (DateTime, Enum, etc.)
  - `Extensions/`: Extension methods and utility functions
  - `Providers/`: High-level query/export interfaces
  - `Utilities/`: Helper classes (ColumnUtils, FileUtils, etc.)
- `Purcell.UnitTest/`: Comprehensive test suite with sample data files
- `Purcell.Benchmarks/`: Performance benchmarking
- `Purcell.ConsoleApp/`: Usage examples and testing

## Key APIs

### Query Operations
```csharp
// Generic typed query
var people = Purcell.Query<Person>("data.xlsx").ToList();

// Dictionary query
var data = Purcell.Query("data.csv").ToList();

// With configuration
var config = PurTable.From("Sheet1").WithHeaderStart("B2");
var results = Purcell.Query<Employee>("file.xlsx", config);
```

### Export Operations
```csharp
// Simple export
Purcell.Export(people, "output.xlsx");

// With configuration
var table = PurTable.From(data, "Results")
    .WithHeaderStart("A1")
    .WithAutoFilter(true)
    .WithPassword("secret");
Purcell.Export(table, "output.xlsx");
```

## Testing

The project uses xUnit for testing with extensive test coverage:
- Unit tests for all major components
- Integration tests with real Excel/CSV files
- Performance benchmarks
- Sample data files in `Resources/` directories

Test files follow naming convention: `Tests_{Operation}_{DataType}.cs`

## File Format Support

- **Excel**: `.xls`, `.xlsx`, `.xlsb` via Sylvan.Data.Excel
- **CSV**: Various encodings (UTF-8, GBK, GB2312) via CsvHelper
- **Large Files**: Optimized for memory efficiency with streaming

## Dependencies

- **Sylvan.Data.Excel**: Excel file reading
- **CsvHelper**: CSV file operations
- **LargeXlsx**: Large Excel file writing
- **System.Text.Encoding.CodePages**: Extended encoding support