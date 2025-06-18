---
# https://vitepress.dev/reference/default-theme-home-page
layout: home

hero:
  name: "Purcell"
  text: "超简单易用的Excel/Csv表格读写ORM库"
  tagline: 超高性能、超低内存占用、极致简单易用
  image:
    src: /logo.png
    alt: Purcell
  actions:
    - theme: brand
      text: 快速开始
      link: /guide/getting-started
    - theme: alt
      text: Purcell简介
      link: /guide/introduction

features:
  - title: O/RM
    icon: 📝
    details: 强类型读写
  - title: 高性能
    icon: 🚀
    details: 逐行读写，超低内存占用，避免OOM
  - title: 易用性
    icon: ✨
    details: Fluent API 设计，简单直观
  - title: 多格式
    icon: 📊
    details: 支持 .xlsx、.xlsb、.xls、.csv 格式
  - title: 零依赖
    icon: 🔧
    details: 纯托管实现，无需安装 Microsoft Office
  - title: 丰富样式
    icon: 🎨
    details: 内置多种预设样式，支持自定义样式
---

<style>
:root {
  --vp-home-hero-name-color: transparent;
  --vp-home-hero-name-background: -webkit-linear-gradient(120deg, #bd34fe 30%, #41d1ff);

  --vp-home-hero-image-background-image: linear-gradient(-45deg, #bd34fe 50%, #47caff 50%);
  --vp-home-hero-image-filter: blur(44px);
}

@media (min-width: 640px) {
  :root {
    --vp-home-hero-image-filter: blur(56px);
  }
}

@media (min-width: 960px) {
  :root {
    --vp-home-hero-image-filter: blur(68px);
  }
}
</style>
