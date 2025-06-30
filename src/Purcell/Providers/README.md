# Purcell Providers

本目录包含 Purcell 库的提供程序架构实现。

## 架构设计

采用分层设计，将原始数据读写与类型映射分离：

```
┌─────────────────┐
│    Adapters     │  ← 映射层：类型映射、转换、验证
├─────────────────┤
│ Format Readers  │  ← 数据层：仅读写 IDictionary<string, object?>
│  (Csv/Excel)    │
└─────────────────┘
```

## 组件说明

### Csv/ 和 Excel/ 目录 - 数据层

**职责**：仅负责文件格式的原始读写，输入输出统一为 `IDictionary<string, object?>`

- **Csv/** - CSV 文件读写器
  - 读写：`IDictionary<string, object?>` 集合
  - 功能：CSV 格式解析、编码处理、分隔符处理
  
- **Excel/** - Excel 文件读写器  
  - 读写：`IDictionary<string, object?>` 集合
  - 功能：Excel 格式解析、工作表处理、单元格类型处理

### Adapters/ 目录 - 映射层

**职责**：提供类型映射、数据转换、数据验证等功能

- 接收来自数据层的 `IDictionary<string, object?>` 
- 执行强类型映射转换
- 提供数据验证和异常处理
- 输出目标类型对象

## 数据流向

```
文件 → Format Readers → IEnumerable<IDictionary<string, object?>> → Adapters → IEnumerable
```

## 设计优势

- **职责清晰**：数据读写与类型映射完全分离
- **易于扩展**：新增文件格式只需实现数据层接口
- **独立测试**：各层可单独进行单元测试