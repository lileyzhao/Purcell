import { defineConfig, type DefaultTheme } from 'vitepress'

// https://vitepress.dev/reference/site-config
export const en = defineConfig({
  lang: 'en-US',
  description: 'Quickly and efficiently process Excel spreadsheets.',
  themeConfig: {
    // https://vitepress.dev/reference/default-theme-config
    nav: nav(),

    sidebar: {
      '/en/guide/': { base: '/en/guide/', items: sidebarGuide() },
    },

    editLink: {
      pattern: 'https://github.com/vuejs/vitepress/edit/main/docs/:path',
      text: 'Edit this page on GitHub'
    },

    footer: {
      message: 'Released under the MIT License.',
      copyright: 'Copyright © 2024-present LileyZhao'
    },
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
    { text: 'Home', link: '/' },
    {
      text: 'Guide',
      link: '/en/guide/introduction',
      activeMatch: '/guide/',
    },
    {
      text: '0.1.0-alpha',
      items: [
        {
          text: 'Changelog',
          link: 'https://github.com/vuejs/vitepress/blob/main/CHANGELOG.md',
        },
        {
          text: 'Contributing',
          link: 'https://github.com/vuejs/vitepress/blob/main/.github/contributing.md',
        },
      ],
    },
  ]
}

function sidebarGuide(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'Introduction',
      collapsed: false,
      items: [
        { text: 'What is Purcell?', link: 'introduction' },
        { text: 'Getting Started', link: 'getting-started' },
        { text: 'Release Notes', link: 'release-notes' },
      ],
    },
    {
      text: 'Usage Guide',
      collapsed: false,
      items: [
        { text: 'Reading Table', link: 'reading-table' },
        { text: 'Exporting Table', link: 'exporting-table' },
      ],
    },
    { text: '🚦 Benchmarking', link: 'benchmarking' },
    { text: '📚 API Reference', link: 'api-reference' },
  ]
}
