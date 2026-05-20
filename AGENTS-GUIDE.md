# AGENTS.md Writing Guide

## English

### Purpose

`AGENTS.md` is a repository instruction file for coding agents. Treat it as an operational companion to `README.md`: the README explains the repository to humans, while `AGENTS.md` tells agents how to work safely and effectively in the repository.

Use it for durable guidance that should apply across many future agent sessions: repository shape, exact commands, validation expectations, coding conventions, boundaries, and contribution workflow.

Do not use it as a dumping ground for everything an agent might ever need. A strong `AGENTS.md` is concise, scoped, current, and grounded in real files and real commands.

### Discovery And Precedence

- Use the canonical filename `AGENTS.md`.
- Place a root `AGENTS.md` at the repository root for repository-wide guidance.
- Add nested `AGENTS.md` files only when a subdirectory has different commands, architecture, risks, generated files, or workflows.
- Guidance is layered by filesystem scope: root guidance applies broadly; nested guidance adds or overrides details for its directory tree.
- When instructions conflict, the more local `AGENTS.md` should win over ancestor files, and explicit user instructions win over all repository guidance.
- Codex-specific note: Codex can also load `AGENTS.override.md` before `AGENTS.md` in the same directory, and can be configured with fallback instruction filenames. Use overrides for temporary or local exceptions, not for shared repository policy.
- Codex-specific note: Codex has a project instruction size limit by configuration, 32 KiB by default, so split large guidance into nested files or link to stable docs when needed.
- Do not repeat inherited rules in child files unless repetition prevents real ambiguity.
- Keep each file small enough to load comfortably. If one file grows too large, split by directory or link to stable documentation.

### What To Include

Prefer sections that change agent behavior:

- **Scope**: what files or directories the document applies to.
- **Context**: a short orientation to what this repository or directory is.
- **Repository structure**: only the directories that matter for safe navigation.
- **Commands**: exact setup, build, lint, test, format, export, or run commands.
- **Validation**: what must be checked before work is considered done.
- **Conventions**: naming, style, architecture, testing, documentation, or review patterns that are specific to this codebase.
- **Boundaries**: generated files, vendored code, secrets, public APIs, historical artifacts, or files that require confirmation before changing.
- **Git and PR workflow**: commit format, scope rules, branch rules, PR checklist, changelog or release notes expectations.
- **Known pitfalls**: concrete traps that agents repeatedly miss.

Write the most useful sections first. Agents frequently need commands and validation criteria more than long background explanations.

### What To Avoid

- Generic assistant behavior such as "be helpful" or "write clean code".
- Invented commands, paths, tools, or directory structures.
- Copying large parts of `README.md`, architecture docs, or human onboarding docs.
- Long essays, vague principles, or rules that cannot be acted on.
- Secrets, credentials, private tokens, or machine-specific values.
- Stale instructions that no longer match the repository.
- Contradictory rules without clear precedence.
- Empty boilerplate sections such as "Testing" when the project has no tests or no known validation command.
- Tool-specific persona prompts. `AGENTS.md` should describe project behavior, not define who the agent is.

### Multi-Level AGENTS.md Practice

Use multi-level files for progressive disclosure:

- Root file: durable repository-wide rules, global workflow, common commands, shared conventions, and commit policy.
- Directory-level file: directory-specific architecture, commands, test commands, generated files, and risks.
- Deep subsystem file: only when a narrow area has special invariants, security constraints, generated artifacts, or high-risk workflows.

A child file should answer: "What is different here that the root file cannot know?" If the answer is "nothing", do not add the child file.

Good child files are smaller than root files. They rely on inherited rules and focus on local differences.

### Claude Compatibility

Claude Code CLI and the Claude Desktop Code tab use `CLAUDE.md` as their project memory file. They do not treat `AGENTS.md` as the canonical project instruction file by default.

To keep one source of truth, use `AGENTS.md` for shared cross-agent guidance and add a minimal `CLAUDE.md` bridge file that imports it:

```markdown
# CLAUDE.md

@AGENTS.md

## Claude Code

- Treat the imported `AGENTS.md` as the shared source of truth.
- Only add Claude-specific notes that cannot live in `AGENTS.md`.
```

Use `@<path-to-file>` anywhere in `CLAUDE.md`. Claude Code supports relative and absolute paths; relative paths resolve from the `CLAUDE.md` file that contains the import. Use the path that matches the filesystem relationship between the bridge file and the target `AGENTS.md`, such as `@AGENTS.md` for the same directory or `@../AGENTS.md` for a parent directory.

For repositories with nested `AGENTS.md` files, mirror the bridge at the same levels where Claude needs local context:

```text
repo/
├── AGENTS.md
├── CLAUDE.md
└── <directory>/
    ├── AGENTS.md
    └── CLAUDE.md
```

Because Claude also loads `CLAUDE.md` files from the directory hierarchy, prefer each bridge file to import only the `AGENTS.md` for its own scope.

Prefer bridge files over duplicating content. Duplicated `AGENTS.md` and `CLAUDE.md` files drift quickly and create unclear precedence.

Keep personal Claude preferences in local, ignored files rather than shared repository instructions.

### Recommended Structure

Use this as a starting point, then delete sections that do not apply:

```markdown
# AGENTS.md

## Scope

- Applies to `<repository-or-directory-path>`.
- Complements parent `AGENTS.md` files; more local guidance wins on conflict.

## Context

- `<one-to-three bullets describing this repository or directory>`

## Commands

- Install: `<exact command>`
- Build: `<exact command>`
- Test all: `<exact command>`
- Test focused change: `<exact command>`

## Validation

- `<checks that must pass before completion>`
- `<manual review or generated artifact checks, if any>`

## Conventions

- `<repository-specific coding, naming, documentation, or design rules>`

## Boundaries

- Never edit `<generated-or-vendored-path>` manually.
- Ask before `<high-risk-action>`.

## Git Workflow

- Commit format: `<format>`
- Scope rules: `<rules>`
```

For very small repositories, `Scope`, `Commands`, `Validation`, and `Git Workflow` may be enough. For non-code projects, replace code commands with the real production or delivery workflow.

### Writing Style

- Use Markdown headings and bullets.
- Prefer imperative, concrete language.
- Use exact commands in code spans or fenced blocks.
- Mention real paths and real files.
- Say "must", "never", or "ask first" only when the rule truly matters.
- Keep paragraphs short.
- Use tables for command maps or scoped file maps when they improve scanability.
- Keep examples context-specific.

### Maintenance Checklist

Review `AGENTS.md` whenever:

- build, test, export, or release commands change;
- a new project, package, or subsystem is added;
- generated files or source-of-truth files move;
- a recurring agent mistake reveals missing guidance;
- commit, PR, changelog, or release rules change;
- a child `AGENTS.md` starts duplicating root guidance;
- the file grows enough that important rules become hard to find.

Before committing an `AGENTS.md` change:

- Confirm every command and path exists or is intentionally documented as external.
- Confirm no secrets or private machine-specific values are included.
- Confirm nested files are scoped correctly.
- Confirm the guide is shorter and more actionable than the documentation it points to.

### Quality Rubric

A good `AGENTS.md` is:

- **Scoped**: clear about where it applies.
- **Actionable**: agents can execute or verify its instructions.
- **Grounded**: based on real repository structure and tools.
- **Layered**: root files stay general; child files stay local.
- **Current**: updated when workflows change.
- **Compact**: high-signal, low-boilerplate.
- **Safe**: highlights irreversible, security-sensitive, generated, or public API boundaries.

### Source References

- Official AGENTS.md site: https://agents.md/
- OpenAI Codex AGENTS.md guide: https://developers.openai.com/codex/guides/agents-md
- AGENTS.md specification discussion: https://github.com/agentsmd/agents.md/issues/135
- Microsoft AGENTS.md generator practice: https://github.com/microsoft/skills/blob/main/.github/plugins/deep-wiki/skills/wiki-agents-md/SKILL.md
- Claude Code memory and import docs: https://code.claude.com/docs/en/memory
- Cloudflare Agents SDK AGENTS.md: https://github.com/cloudflare/agents/blob/main/AGENTS.md
- OpenAI Cookbook AGENTS.md: https://github.com/openai/openai-cookbook/blob/main/AGENTS.md
- OpenAI Agents Python AGENTS.md: https://github.com/openai/openai-agents-python/blob/main/AGENTS.md
- Apache Airflow providers AGENTS.md: https://github.com/apache/airflow/blob/main/providers/AGENTS.md
- Temporal Java SDK AGENTS.md: https://github.com/temporalio/sdk-java/blob/master/AGENTS.md
- Google ADK Python AGENTS.md: https://github.com/google/adk-python/blob/main/AGENTS.md

## 中文

### 目的

`AGENTS.md` 是写给 coding agent 的仓库指令文件。可以把它理解为 `README.md` 的操作型搭档：README 面向人解释仓库，`AGENTS.md` 面向 agent 说明如何在仓库里安全、有效地工作。

它适合放那些会在未来多次 agent 会话中反复生效的稳定指导：仓库形态、精确命令、验证要求、代码约定、边界和协作流程。

不要把它写成 agent 可能需要的一切资料集合。好的 `AGENTS.md` 应该简洁、有作用域、保持更新，并且基于真实文件和真实命令。

### 发现规则与优先级

- 使用标准文件名 `AGENTS.md`。
- 在仓库根目录放置根级 `AGENTS.md`，用于仓库级规则。
- 只有当某个子目录有不同命令、架构、风险、生成文件或流程时，才添加嵌套 `AGENTS.md`。
- 指令按文件系统作用域分层：根级规则广泛适用；子级规则为对应目录树补充或覆盖细节。
- 指令冲突时，更靠近当前文件的 `AGENTS.md` 优先；用户明确指令优先于所有仓库文档。
- Codex 特定说明：Codex 在同一目录中会先读取 `AGENTS.override.md`，再读取 `AGENTS.md`，也可以配置 fallback 指令文件名。override 适合临时或本地例外，不适合承载共享仓库规范。
- Codex 特定说明：Codex 的项目指令有配置层面的大小上限，默认是 32 KiB；必要时应拆成嵌套文件，或链接到稳定文档。
- 子级文件不要重复继承来的规则，除非重复能避免真实歧义。
- 每个文件都应保持在容易加载和阅读的长度内；如果内容过长，按目录拆分，或链接到稳定文档。

### 应该包含什么

优先写能改变 agent 行为的内容：

- **适用范围**：文档适用于哪些文件或目录。
- **上下文**：用很短的说明让 agent 知道仓库或目录是什么。
- **仓库结构**：只列出影响安全导航的重要目录。
- **命令**：精确的安装、构建、lint、测试、格式化、导出或运行命令。
- **验证要求**：什么检查完成后，工作才算可以交付。
- **约定**：所在仓库特有的命名、风格、架构、测试、文档或评审规则。
- **边界**：生成文件、vendor 代码、密钥、公开 API、历史产物，或需要确认才能修改的文件。
- **Git 和 PR 流程**：提交格式、scope 规则、分支规则、PR checklist、changelog 或 release notes 要求。
- **已知陷阱**：agent 容易反复犯错的具体问题。

最有用的内容放前面。agent 通常更需要命令和完成标准，而不是很长的背景说明。

### 不应该包含什么

- 类似“be helpful”或“write clean code”的通用 assistant 行为。
- 不存在的命令、路径、工具或目录结构。
- 大段复制 `README.md`、架构文档或人工 onboarding 文档。
- 长篇论述、模糊原则，或无法执行的规则。
- 密钥、凭据、私有 token 或绑定某台机器的值。
- 已经过期、与仓库不一致的说明。
- 没有明确优先级的互相矛盾规则。
- 空泛模板章节，例如项目没有测试却硬写 “Testing”。
- 工具专属人格提示。`AGENTS.md` 应描述项目行为规则，而不是定义 agent 是谁。

### 多层 AGENTS.md 实践

多层文件的核心是渐进披露：

- 根级文件：长期稳定的仓库级规则、全局流程、通用命令、共享约定和提交规范。
- 目录级文件：目录特定架构、命令、测试命令、生成文件和风险。
- 更深层子系统文件：只有当某个窄范围有特殊不变量、安全约束、生成产物或高风险流程时才添加。

写子级文件前先问一句：“这里有什么是根级文件无法知道的差异？” 如果答案是“没有”，就不要新增子级文件。

好的子级文件通常比根级文件更短。它依赖继承来的规则，只聚焦本地差异。

### Claude 兼容

Claude Code CLI 和 Claude Desktop 的 Code tab 使用 `CLAUDE.md` 作为项目记忆文件。它们默认不会把 `AGENTS.md` 当作标准项目指令文件读取。

为了保持单一事实来源，建议把 `AGENTS.md` 作为跨 agent 的共享规范，再添加一个最小化的 `CLAUDE.md` 桥接文件来导入它：

```markdown
# CLAUDE.md

@AGENTS.md

## Claude Code

- Treat the imported `AGENTS.md` as the shared source of truth.
- Only add Claude-specific notes that cannot live in `AGENTS.md`.
```

可以在 `CLAUDE.md` 的任意位置使用 `@<path-to-file>`。Claude Code 支持相对路径和绝对路径；相对路径从包含该 import 的 `CLAUDE.md` 文件解析。根据桥接文件与目标 `AGENTS.md` 的文件系统位置关系填写路径，例如同目录用 `@AGENTS.md`，父目录用 `@../AGENTS.md`。

如果仓库中有多层 `AGENTS.md`，应在 Claude 需要本地上下文的相同层级放置桥接文件：

```text
repo/
├── AGENTS.md
├── CLAUDE.md
└── <directory>/
    ├── AGENTS.md
    └── CLAUDE.md
```

由于 Claude 也会按目录层级加载 `CLAUDE.md`，每个桥接文件通常只导入自己作用域内的 `AGENTS.md`。

优先使用桥接文件，不要维护两份重复内容。重复的 `AGENTS.md` 和 `CLAUDE.md` 很容易漂移，并让优先级变得不清楚。

个人 Claude 偏好应放在本地忽略文件中，不要写进共享仓库指令。

### 推荐结构

可以从这个模板开始，然后删除不适用的章节：

```markdown
# AGENTS.md

## Scope

- Applies to `<repository-or-directory-path>`.
- Complements parent `AGENTS.md` files; more local guidance wins on conflict.

## Context

- `<one-to-three bullets describing this repository or directory>`

## Commands

- Install: `<exact command>`
- Build: `<exact command>`
- Test all: `<exact command>`
- Test focused change: `<exact command>`

## Validation

- `<checks that must pass before completion>`
- `<manual review or generated artifact checks, if any>`

## Conventions

- `<repository-specific coding, naming, documentation, or design rules>`

## Boundaries

- Never edit `<generated-or-vendored-path>` manually.
- Ask before `<high-risk-action>`.

## Git Workflow

- Commit format: `<format>`
- Scope rules: `<rules>`
```

对于很小的仓库，`Scope`、`Commands`、`Validation` 和 `Git Workflow` 可能已经足够。对于非代码项目，把代码命令替换成真实的生产或交付流程。

### 写作风格

- 使用 Markdown 标题和列表。
- 优先使用命令式、具体的表达。
- 命令放在行内代码或代码块里。
- 提到真实路径和真实文件。
- 只有规则真的重要时，才使用 “must”、“never” 或 “ask first”。
- 段落保持简短。
- 当表格能提升可扫读性时，用表格整理命令或作用域映射。
- 示例应该贴合当前上下文，不要使用泛化模板。

### 维护检查清单

以下情况应检查 `AGENTS.md`：

- build、test、export 或 release 命令变化；
- 新增项目、package 或子系统；
- 生成文件或事实来源文件移动；
- agent 反复犯同一种错误，说明缺少指导；
- commit、PR、changelog 或 release 规则变化；
- 子级 `AGENTS.md` 开始重复根级规则；
- 文件变长，重要规则变得难以找到。

提交 `AGENTS.md` 变更前：

- 确认每个命令和路径都真实存在，或明确说明它是外部依赖。
- 确认没有密钥或绑定个人机器的私有值。
- 确认嵌套文件作用域正确。
- 确认这份指南比它引用的文档更短、更可执行。

### 质量标准

好的 `AGENTS.md` 应该是：

- **有作用域的**：清楚说明适用范围。
- **可执行的**：agent 能执行或验证其中的指令。
- **有依据的**：基于真实仓库结构和工具。
- **分层的**：根级文件保持通用，子级文件保持本地化。
- **保持更新的**：工作流变化时同步更新。
- **紧凑的**：高信号、低模板化。
- **安全的**：明确不可逆、安全敏感、生成文件或公开 API 边界。

### 参考资料

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
