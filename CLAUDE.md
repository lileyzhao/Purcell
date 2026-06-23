# CLAUDE.md

本文件是 Claude Code 在本仓库中的兼容入口（compatibility bridge）。共享项目规范不在这里重复维护；请把导入的 `AGENTS.md` 视为事实来源（source of truth）。

@AGENTS.md

## Claude Code

- 默认使用中文沟通（Chinese first）；必要术语、命令、代码、配置键名和关键操作概念保留英文关键词（English keyword anchors）。
- `CLAUDE.md` 只承担 Claude Code 兼容桥接职责；跨 agent 的长期规则应写入 `AGENTS.md` 或 `AGENTS.override.md`，不要在这里复制第二份完整规范。
- Claude Code 默认读取 `CLAUDE.md`，不等同于 Codex 的 project docs 自动发现机制。处理子目录任务前，应从仓库根目录到目标工作目录逐层补读适用的 `AGENTS.md`；若存在 `AGENTS.override.md`，按当前工具和仓库约定理解它的本地覆盖语义（local override）。
- 其他工具可能通过自己的配置声明 fallback 指令文件；这些配置只对对应工具生效，不代表 Claude Code 会自动解析或继承。
- 只有确实无法放入 `AGENTS.md` 的 Claude Code 专属说明，才应该继续写在本文件中。
