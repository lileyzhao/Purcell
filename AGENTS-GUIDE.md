# AGENTS.md 编写指南

这份指南用于生成和维护高质量的 `AGENTS.md`。它面向中文使用者，但会在关键概念处保留少量英文关键词（English keyword anchors），帮助 coding agent 更精准地理解规则含义。

## 目标

`AGENTS.md` 是写给 coding agent 的仓库指令文件。可以把它理解为 `README.md` 的操作型搭档：`README.md` 面向人解释项目是什么，`AGENTS.md` 面向 agent 说明如何在项目里安全、有效地工作。

它适合承载会在未来多次会话中反复生效的稳定规则，例如仓库结构、真实命令、验证要求、编码约定、边界、协作流程和提交规范。

不要把 `AGENTS.md` 写成资料全集。好的 `AGENTS.md` 应该是：

- 有作用域（scoped）
- 可执行（actionable）
- 可验证（verifiable）
- 基于真实仓库（grounded）
- 简洁但不含糊（compact but precise）
- 能降低 agent 犯错概率

## 核心判断

写入 `AGENTS.md` 前先判断这条内容是否满足至少一个条件：

- 它会改变 agent 的行为。
- 它能避免 agent 反复犯错。
- 它能说明真实命令、真实路径或真实边界。
- 它是长期稳定规则，而不是一次性任务说明。
- 它比让 agent 自己猜更安全。

如果只是背景知识、临时偏好、长篇解释、人工 onboarding 内容，通常不应该放进 `AGENTS.md`。

## 中文优先与理解精度

中文表达应服务于中文使用者的阅读顺畅，但不得降低 agent 对规则的理解精度（operational precision）。

- 改写既有规范前，先理解原文的 `scope`、优先级、触发条件、禁止事项、验证方式和事实来源（source of truth），再重写表达；不要做机械翻译、机械精简或术语替换。
- 英文关键词（English keyword anchors）只用于锚定容易误读的操作概念，例如 `scope`、`source of truth`、`minimal diff`、`validation`、`do not touch`、`fallback`；少而准，不堆术语。
- 改写完成后从两侧检查：中文使用者能自然读懂，agent 也能明确判断“何时适用、应该做什么、不能做什么、如何验证”。
- 如果中文表达和 agent 执行精度发生冲突，优先重写句子结构、补充触发条件或加入英文锚点，而不是牺牲任一侧。

## 发现规则与优先级

- 使用标准文件名 `AGENTS.md`。
- 在仓库根目录放置根级 `AGENTS.md`，用于仓库级规则。
- 只有当子目录存在不同命令、架构、风险、生成文件或工作流时，才添加嵌套 `AGENTS.md`。
- 指令按文件系统作用域分层（layered by scope）：根级规则广泛适用；子级规则为对应目录树补充或覆盖细节。
- 指令冲突时，更靠近当前文件的 `AGENTS.md` 优先（local wins）；用户明确指令优先于所有仓库文档（user instructions win）。
- 子级文件不要重复继承来的规则，除非重复能避免真实歧义。
- 每个文件都应保持容易加载和阅读；如果内容变长，优先按目录拆分为嵌套 `AGENTS.md`，而不是堆到根文件。

工具兼容补充（非标准主体）：

- 以下内容用于处理具体工具的加载差异，不是 `AGENTS.md` 标准结构的一部分；只有仓库实际依赖相关工具时，才写入共享指南或桥接文件。
- 例如 Codex 在同一目录中可加载 `AGENTS.override.md` 与 `AGENTS.md`；`override` 适合临时或本地例外，不适合承载共享仓库规范。
- 一些工具可配置 fallback 指令文件名或项目指令大小上限；这类配置应作为工具兼容说明处理，不要覆盖标准 `AGENTS.md` 的事实来源（source of truth）地位。
- 普通“被引用的说明文件”不等于会被 agent 自动读取。关键行为规则应写进会被稳定加载的 `AGENTS.md` 层级；外部文档只适合承载详细背景或低频资料。

## 应该包含什么

优先写能直接改变 agent 行为的内容。常见有效章节包括：

- **适用范围（Scope）**：文档适用于哪些文件、目录或子系统。
- **上下文（Context）**：用 1-3 条说明仓库或目录是什么，避免长背景。
- **仓库结构（Repository structure）**：只列出影响安全导航的重要目录。
- **命令（Commands）**：精确的安装、构建、lint、测试、格式化、导出或运行命令。
- **验证要求（Validation）**：哪些检查通过后，工作才算可以交付。
- **约定（Conventions）**：仓库特有的命名、风格、架构、测试、文档或评审规则。
- **边界（Boundaries）**：生成文件、vendor 代码、密钥、公开 API、历史产物，或必须确认后才能修改的文件。
- **协作策略（Workflow）**：review、handoff、PR、changelog 或 release notes 等流程要求。
- **Git 规范（Git workflow）**：commit format、scope 规则、分支规则、提交范围。
- **已知陷阱（Known pitfalls）**：agent 容易误判或反复犯错的具体问题。

把最有用、最常用的内容放前面。agent 通常优先需要命令、边界和完成标准，而不是长篇背景。

## 不应该包含什么

避免把以下内容写进 `AGENTS.md`：

- “be helpful”“write clean code” 这类通用 assistant 行为。
- 不存在的命令、路径、工具或目录结构。
- 大段复制 `README.md`、架构文档或人工 onboarding 文档。
- 长篇论述、抽象口号、无法执行的原则。
- 密钥、凭据、token 或绑定个人机器的私有值。
- 已经过期、与仓库不一致的说明。
- 没有明确优先级的冲突规则。
- 空泛模板章节，例如项目没有测试却硬写 “Testing”。
- 工具专属人格提示。`AGENTS.md` 应描述项目行为，不应定义 agent 的人格。
- 只在当前会话有效的一次性计划。一次性约束应放在用户 prompt 或当前任务说明中。

## 多层 AGENTS.md 实践

多层文件的核心是渐进披露（progressive disclosure）：

- 根级文件：长期稳定的仓库级规则、全局流程、通用命令、共享约定和提交规范。
- 目录级文件：目录特定架构、命令、测试方式、生成文件、风险和边界。
- 深层子系统文件：只有当窄范围存在特殊不变量（invariants）、安全约束、生成产物或高风险流程时才添加。

写子级文件前先问：

> 这里有什么是根级文件无法准确表达的本地差异？

如果答案是“没有”，不要新增子级文件。好的子级文件通常比根级文件更短，依赖继承规则，只聚焦本地差异。

不要为了“整理得更漂亮”把一个短小根文件拆成多个普通说明文件。拆分必须服务于加载机制、作用域或可维护性。

## 推荐结构

从下面模板开始，然后删除不适用的章节。不要为了模板完整而保留空章节。

```markdown
# AGENTS.md

## 适用范围（Scope）

- 适用于 `<repository-or-directory-path>`。
- 与父级 `AGENTS.md` 叠加生效；发生冲突时，更靠近当前文件的规则优先（local wins）。

## 项目上下文（Context）

- `<用 1-3 条说明本仓库或目录的职责、关键边界或运行环境>`

## 常用命令（Commands）

- 安装依赖（Install）：`<exact command>`
- 构建（Build）：`<exact command>`
- 全量测试（Test all）：`<exact command>`
- 聚焦验证（Test focused change）：`<exact command>`

## 验证要求（Validation）

- `<完成前必须通过的检查，例如 lint、test、typecheck 或 manual QA>`
- `<需要人工检查、截图确认或生成产物比对的步骤>`

## 项目约定（Conventions）

- `<仓库特有的代码、命名、文档、架构、测试或 UI 规则>`

## 修改边界（Boundaries）

- 不要手动编辑 `<generated-or-vendored-path>`（generated/vendor）。
- 执行 `<high-risk-action>` 前先询问（ask first）。

## Git 规范（Git Workflow）

- 提交格式（commit format）：`<format>`
- 提交范围（scope rules）：`<rules>`
```

小仓库可以只保留 `适用范围（Scope）`、`常用命令（Commands）`、`验证要求（Validation）` 和 `Git 规范（Git Workflow）`。非代码项目应把代码命令替换为真实生产、交付或验收流程。

如果需要，可以再加入 `仓库结构（Repository structure）`、`协作策略（Workflow）`、`已知陷阱（Known pitfalls）` 等可选章节；前提是这些内容真实改变 agent 行为，而不是为了模板完整。

## 写作风格

- 使用 Markdown 标题和列表。
- 优先使用命令式、具体表达。
- 命令、路径、配置键、文件名放在行内代码或代码块里。
- 提到真实路径、真实文件和真实命令；不确定时不要编造。
- 中文为主时，给容易被 agent 误解的概念补少量英文关键词（English keyword anchors），例如 `scope`、`ownership`、`acceptance criteria`、`minimal diff`、`validation`、`source of truth`、`do not touch`、`user-visible behavior`。
- 英文关键词用于锚定操作语义，不要机械翻译每句话，也不要让文档变成中英混杂的术语堆。
- 只有规则真的重要时，才使用 “must”、“never”、“ask first” 或“必须/禁止/先询问”。
- 段落保持短，列表保持高信号。
- 表格只在能提升可扫读性时使用，例如命令表、目录作用域表、风险边界表。
- 示例应贴合当前仓库，不要使用泛化模板。
- 用“不要做什么”时，尽量说明触发条件或替代做法，避免只写抽象禁令。

## 跨工具兼容（Tool Compatibility）

`AGENTS.md` 是跨 agent 的共享规范。不同工具的读取机制可能不同，应尽量保持一个事实来源（single source of truth）。

本节是多工具协作的补充说明，不是每个 `AGENTS.md` 都必须包含的标准章节。只有仓库确实同时服务多个 coding agents 时，才需要桥接文件或工具说明。

Claude Code CLI 和 Claude Desktop 的 Code tab 默认使用 `CLAUDE.md` 作为项目记忆文件。为了避免维护两套内容，建议添加最小桥接文件：

```markdown
# CLAUDE.md

本文件是 Claude Code 的兼容入口（compatibility bridge）。共享项目规范不在这里重复维护；请把导入的 `AGENTS.md` 视为事实来源（source of truth）。

@AGENTS.md

## Claude Code

- 默认使用中文沟通（Chinese first）；必要术语、命令、代码、配置键名和关键操作概念保留英文关键词（English keyword anchors）。
- 只有确实无法放入 `AGENTS.md` 的 Claude Code 专属说明，才应该继续写在本文件中。
```

如果仓库中有多层 `AGENTS.md`，需要 Claude 在对应目录读取本地规则时，可在相同层级放置最小 `CLAUDE.md` 桥接文件，并只导入该层级自己的 `AGENTS.md`。

不要复制两份完整规则。重复的 `AGENTS.md` 和 `CLAUDE.md` 很容易漂移，导致优先级和事实来源不清。

## 维护检查清单

以下情况应检查并更新 `AGENTS.md`：

- build、test、export、deploy 或 release 命令变化。
- 新增 package、子项目或子系统。
- 生成文件、事实来源文件或重要目录移动。
- agent 反复犯同一种错误，说明缺少指导。
- commit、PR、changelog 或 release 规则变化。
- 子级 `AGENTS.md` 开始重复根级规则。
- 文件变长，重要规则变得难以找到。
- 关键技术栈、权限模型、公开 API 或数据边界变化。

提交 `AGENTS.md` 变更前确认：

- 每个命令和路径都真实存在，或明确说明它是外部依赖。
- 没有密钥、token 或绑定个人机器的私有值。
- 嵌套文件作用域正确。
- 没有和父级或子级规则产生未解释的冲突。
- 文档比它引用的资料更短、更可执行。
- 新增内容能改变 agent 行为，而不是只增加背景。

## 质量标准

好的 `AGENTS.md` 应该满足：

- **Scoped**：清楚说明适用范围。
- **Actionable**：agent 能按它执行。
- **Verifiable**：agent 能知道如何验证完成。
- **Grounded**：基于真实仓库结构、真实命令和真实文件。
- **Layered**：根级文件保持通用，子级文件聚焦本地差异。
- **Current**：工作流变化时同步更新。
- **Compact**：高信号、低模板化。
- **Safe**：明确不可逆、安全敏感、生成文件、公开 API 和数据边界。
- **Readable**：中文使用者读起来顺，agent 也能通过英文关键词精准抓住操作语义。

## 参考资料

- 官方 AGENTS.md 站点：https://agents.md/
- OpenAI Codex AGENTS.md 指南：https://developers.openai.com/codex/guides/agents-md
- AGENTS.md 规范讨论：https://github.com/agentsmd/agents.md/issues/135
- Microsoft AGENTS.md 生成器实践：https://github.com/microsoft/skills/blob/main/.github/plugins/deep-wiki/skills/wiki-agents-md/SKILL.md
- Claude Code memory 与 import 文档：https://code.claude.com/docs/en/memory
- Cloudflare Agents SDK AGENTS.md：https://github.com/cloudflare/agents/blob/main/AGENTS.md
- OpenAI Cookbook AGENTS.md：https://github.com/openai/openai-cookbook/blob/main/AGENTS.md
- OpenAI Agents Python AGENTS.md：https://github.com/openai/openai-agents-python/blob/main/AGENTS.md
- Apache Airflow providers AGENTS.md：https://github.com/apache/airflow/blob/main/providers/AGENTS.md
- Temporal Java SDK AGENTS.md：https://github.com/temporalio/sdk-java/blob/master/AGENTS.md
- Google ADK Python AGENTS.md：https://github.com/google/adk-python/blob/main/AGENTS.md
