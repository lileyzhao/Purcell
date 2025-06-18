import { defineConfig, type DefaultTheme } from 'vitepress'

// https://vitepress.dev/reference/site-config
export const zh = defineConfig({
  lang: 'zh-Hans',
  description: '快速有效地处理Excel电子表格。',
  themeConfig: {
    // https://vitepress.dev/reference/default-theme-config
    nav: nav(),

    sidebar: {
      '/guide/': { base: '/guide/', items: sidebarGuide() },
    },

    editLink: {
      pattern: 'https://github.com/vuejs/vitepress/edit/main/docs/:path',
      text: '在 GitHub 上编辑此页面',
    },

    footer: {
      message: '基于 MIT 许可发布',
      copyright: `版权所有 © 2024-${new Date().getFullYear()} LileyZhao`,
    },

    docFooter: {
      prev: '上一页',
      next: '下一页',
    },

    outline: { label: '页面导航', level: [2, 4] },

    lastUpdated: {
      text: '最后更新于',
      formatOptions: {
        dateStyle: 'short',
        timeStyle: 'medium',
      },
    },

    langMenuLabel: '多语言',
    returnToTopLabel: '回到顶部',
    sidebarMenuLabel: '菜单',
    darkModeSwitchLabel: '主题',
    lightModeSwitchTitle: '切换到浅色模式',
    darkModeSwitchTitle: '切换到深色模式',
    // skipToContentLabel: '跳转到内容',
  },
  locales: {
    root: {
      label: '简体中文',
      lang: 'zh',
    },
    fr: {
      label: 'English',
      lang: 'en', // 可选，将作为 `lang` 属性添加到 `html` 标签中
      link: '/en/', // 默认 /fr/ -- 显示在导航栏翻译菜单上，可以是外部的
    },
  },
})

function nav(): DefaultTheme.NavItem[] {
  return [
    { text: '首页', link: '/' },
    {
      text: '指南',
      link: '/guide/introduction',
      activeMatch: '/guide/',
    },
    {
      text: '0.1.0-alpha',
      items: [
        {
          text: '更新日志',
          link: 'https://github.com/vuejs/vitepress/blob/main/CHANGELOG.md',
        },
        {
          text: '参与贡献',
          link: 'https://github.com/vuejs/vitepress/blob/main/.github/contributing.md',
        },
      ],
    },
  ]
}

function sidebarGuide(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: '开始使用',
      collapsed: false,
      items: [
        { text: 'Purcell是什么？', link: 'introduction' },
        { text: '快速开始', link: 'getting-started' },
      ],
    },
    {
      text: '使用指南',
      collapsed: false,
      items: [
        { text: '读取表格', link: 'reading-table' },
        { text: '导出表格', link: 'exporting-table' },
      ],
    },
    {
      text: '进阶内容',
      collapsed: false,
      items: [
        { text: '🚦 基准测试', link: 'benchmarking' },
        { text: '📚 API 参考', link: 'api-reference' },
      ],
    },
    { text: '更新日志', link: 'release-notes' },
  ]
}
