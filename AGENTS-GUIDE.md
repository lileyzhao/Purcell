# AGENTS.md 编写指南

本指南规定如何维护仓库中的 `AGENTS.md`。`README.md` 解释项目，`AGENTS.md` 约束 coding agent 在项目中的行为；它不是需求或架构文档的副本。中文表述应自然、明确；只在容易误读的操作概念上使用少量英文关键词（如 `scope`、`source of truth`、`validation`）。

## 选取规则

一条规范应长期有效，并至少能改变 agent 的行为、避免已知错误，或明确真实命令、路径、边界与验证要求。写入前核对其适用范围、触发条件、优先级、禁止事项和事实来源。临时任务、历史过程、未确认假设、长篇背景和人工 onboarding 内容不放进 `AGENTS.md`；详细需求与设计留在各自的权威文档中。

保留无法安全依赖 agent 自行推断的关键约束。精简时可删除重复表述、无用示例或会漂移的统计数字，但不能丢失有效规则的条件、例外和执行力度。引用型文档不会因此自动加载：如果缺少某条规则会使 agent 在读取详细文档前就作出错误决策，应在适用的 `AGENTS.md` 中保留简短、明确的规则，并标出何时阅读详情。

## 文档职责与事实来源

- `AGENTS.md`：记录 agent 在仓库中必须执行的行为约束，包括适用范围、操作顺序、禁令、验证和交付要求；只补充安全执行任务所必需的项目上下文。
- 目录 `README.md`：记录目录职责、入口、边界和使用方式；根目录 README 负责项目概览和导航。
- `docs/` 或其他明确的权威文档：记录跨目录架构、协议、数据契约和技术决策。
- 代码、配置和 schema：分别作为实现与运行事实的直接来源。`AGENTS.md` 不复制实现细节，只在这些事实会改变 agent 行为时保留简短、可执行的约束和链接。

同一项职责、契约或实现只保留一个正式定义，其他位置只保留必要摘要和链接，避免内容漂移。链接不能替代 agent 在执行前就必须知道的关键约束；这类约束应在适用的 `AGENTS.md` 中保留，并指向详细定义。

## 作用域与加载

- 仓库根级 `AGENTS.md` 记录全局规则；仅当子目录有独立命令、架构、生成文件、工作流或风险边界时，才添加子级文件。子级补充或覆盖父级规则，冲突时更靠近目标文件的规则优先（local wins）；用户明确指令优先。
- 优先在现有文件中按主题分条整理，不为同一目录的主题另建普通说明文件，也不为了拆分而增加规范层级。子级不重复父级，除非简短提醒确能防止具体误解。
- Codex 在同一目录优先选用 `AGENTS.override.md`，有该文件时不再叠加该层的 `AGENTS.md`；`override` 仅适合临时或本地例外，不承载共享长期规范。工具可能另设 fallback 文件名或指令大小上限，必须按其实际配置检查从根到目标目录的可加载内容；将超限正文拆进下级文件不一定能降低累计大小。
- Claude Code 使用 `CLAUDE.md` 作为本仓库的兼容入口并导入根级 `AGENTS.md`。处理子目录文件时，须逐层读取适用的子级 `AGENTS.md`；不要假设其他工具的 fallback 或 override 会被 Claude 自动解析。保持 `CLAUDE.md` 为最小桥接，不复制共享规则。

## 内容与写法

优先记录真实、可操作且可复查的信息：适用范围、必要上下文、精确的构建与测试命令、完成标准、局部架构约定、生成文件与密钥边界、公开接口兼容性、风险操作和 Git 流程。最常用的命令、边界和完成标准应靠前；章节仅在有实际内容时设置，不套用空模板。

- 一条规则尽量只表达一个决策。说明“何时适用、谁做什么、不得做什么、如何验证”；涉及相邻条件或时序时不要依赖省略的主语或含糊的“其”“本次”。
- 命令、路径、文件名和配置键使用准确的行内代码或代码块。不要写不存在的路径、绑定个人机器的值、密钥或 token。
- 只在规则真的强制时使用“必须”“禁止”“先询问”；禁令尽量说明触发条件或替代做法。避免泛化口号、逐句英译和中英术语堆砌。
- 短段落和列表优先；只有表格能改善扫描时才用表格。示例应来自实际仓库，不能以示例数据替代事实。
- 改写后分别检查：人类能自然阅读；agent 能确定适用范围、操作、禁令和验证；翻成英文时主体、条件、例外和时序仍明确。

可按需要使用以下结构，不要求每份文件都包含所有章节：`适用范围`、`项目上下文`、`常用命令`、`验证要求`、`项目约定`、`修改边界`、`Git 规范`。小目录只保留有独立约束的章节。

## 维护检查

在命令、目录、子系统、权限、公开 API 或生成方式变化，以及 agent 重复犯错时，检查受影响层级的 `AGENTS.md` 和相关权威文档。修改前确认现有规则没有在父子级重复、过时或互相冲突；修改后检查：

- 所列命令和路径真实存在，验证要求可以执行；不能验证的事实明确说明。
- 每条规则有清楚作用域和长期价值，且比其引用的详细资料更短、更可操作。
- 目录或文件结构发生变化时，确认相关目录 README、局部 `AGENTS.md`、构建入口和文档链接是否仍反映真实职责；不要只更新路径而遗漏边界说明。
- 涉及新增文件或目录的规范时，确认适用的 `AGENTS.md` 要求 agent 在创建前审查职责、归属、边界、命名和复用；具体目录职责与结构事实仍由对应 README 或权威设计文档定义。不要把文档维护建议误写成项目运行规则，也不要把架构事实泛化为 agent 禁令。
- 没有弱化安全、权限、数据一致性、生成文件和高风险操作的约束。
- 子目录任务所需规范仍能被目标工具加载，且没有越过其大小限制。

## 参考资料

- 官方 AGENTS.md 站点：https://agents.md/
- OpenAI Codex AGENTS.md 指南：https://developers.openai.com/codex/guides/agents-md
- Claude Code memory 与 import 文档：https://code.claude.com/docs/en/memory
- AGENTS.md 规范讨论：https://github.com/agentsmd/agents.md/issues/135
- Microsoft AGENTS.md 生成器实践：https://github.com/microsoft/skills/blob/main/.github/plugins/deep-wiki/skills/wiki-agents-md/SKILL.md
- Cloudflare Agents SDK AGENTS.md：https://github.com/cloudflare/agents/blob/main/AGENTS.md
- OpenAI Cookbook AGENTS.md：https://github.com/openai/openai-cookbook/blob/main/AGENTS.md
- OpenAI Agents Python AGENTS.md：https://github.com/openai/openai-agents-python/blob/main/AGENTS.md
- Apache Airflow providers AGENTS.md：https://github.com/apache/airflow/blob/main/providers/AGENTS.md
- Temporal Java SDK AGENTS.md：https://github.com/temporalio/sdk-java/blob/master/AGENTS.md
- Google ADK Python AGENTS.md：https://github.com/google/adk-python/blob/main/AGENTS.md
