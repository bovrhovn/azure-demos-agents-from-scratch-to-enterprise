<template>
  <header class="relative border-b border-white/5 bg-slate-900/80 backdrop-blur-lg">
    <!-- Ambient glow -->
    <div class="absolute inset-0 bg-gradient-to-r from-sky-600/5 via-transparent to-cyan-600/5 pointer-events-none" />

    <div class="relative mx-auto max-w-4xl px-6 py-4 flex items-center justify-between">
      <!-- Brand -->
      <RouterLink to="/" class="flex items-center gap-3 hover:opacity-90 transition-opacity">
        <div class="flex items-center justify-center w-8 h-8 rounded-lg bg-sky-600/20 border border-sky-500/30">
          <svg class="w-4 h-4 text-sky-400" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
          </svg>
        </div>
        <div>
          <h1 class="text-sm font-semibold text-slate-100 tracking-wide">Enterprise Search</h1>
          <p class="text-xs text-slate-500">Powered by ASE</p>
        </div>
      </RouterLink>

      <!-- Navigation -->
      <nav class="flex items-center gap-1" aria-label="Main navigation">
        <RouterLink
          to="/"
          class="nav-link"
          :class="{ 'nav-link-active': isActive('/') }"
          aria-label="Basic Search"
        >
          <svg class="w-3.5 h-3.5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
          </svg>
          Basic
        </RouterLink>

        <RouterLink
          to="/advanced"
          class="nav-link"
          :class="{ 'nav-link-active': isActive('/advanced') }"
          aria-label="Advanced Search"
        >
          <svg class="w-3.5 h-3.5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="M6 13.5V3.75m0 9.75a1.5 1.5 0 0 1 0 3m0-3a1.5 1.5 0 0 0 0 3m0 3.75V16.5m6-10.5V3.75m0 2.25a1.5 1.5 0 0 1 0 3m0-3a1.5 1.5 0 0 0 0 3m0 9.75V8.25m6 2.25V3.75m0 6.75a1.5 1.5 0 0 1 0 3m0-3a1.5 1.5 0 0 0 0 3m0 3.75V13.5" />
          </svg>
          Advanced
        </RouterLink>

        <div class="w-px h-4 bg-white/10 mx-1" />

        <RouterLink
          to="/settings"
          class="nav-link"
          :class="{ 'nav-link-active': isActive('/settings') }"
          aria-label="Settings"
        >
          <svg class="w-3.5 h-3.5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="M9.594 3.94c.09-.542.56-.94 1.11-.94h2.593c.55 0 1.02.398 1.11.94l.213 1.281c.063.374.313.686.645.87.074.04.147.083.22.127.325.196.72.257 1.075.124l1.217-.456a1.125 1.125 0 0 1 1.37.49l1.296 2.247a1.125 1.125 0 0 1-.26 1.431l-1.003.827c-.293.241-.438.613-.43.992a7.723 7.723 0 0 1 0 .255c-.008.378.137.75.43.991l1.004.827c.424.35.534.955.26 1.43l-1.298 2.247a1.125 1.125 0 0 1-1.369.491l-1.217-.456c-.355-.133-.75-.072-1.076.124a6.47 6.47 0 0 1-.22.128c-.331.183-.581.495-.644.869l-.213 1.281c-.09.543-.56.94-1.11.94h-2.594c-.55 0-1.019-.398-1.11-.94l-.213-1.281c-.062-.374-.312-.686-.644-.87a6.52 6.52 0 0 1-.22-.127c-.325-.196-.72-.257-1.076-.124l-1.217.456a1.125 1.125 0 0 1-1.369-.49l-1.297-2.247a1.125 1.125 0 0 1 .26-1.431l1.004-.827c.292-.24.437-.613.43-.991a6.932 6.932 0 0 1 0-.255c.007-.38-.138-.751-.43-.992l-1.004-.827a1.125 1.125 0 0 1-.26-1.43l1.297-2.247a1.125 1.125 0 0 1 1.37-.491l1.216.456c.356.133.751.072 1.076-.124.072-.044.146-.086.22-.128.332-.183.582-.495.644-.869l.214-1.28Z" />
            <path stroke-linecap="round" stroke-linejoin="round" d="M15 12a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z" />
          </svg>
          Settings
        </RouterLink>
      </nav>
    </div>
  </header>
</template>

<script setup lang="ts">
import { RouterLink, useRoute } from 'vue-router'

const route = useRoute()

function isActive(path: string): boolean {
  return route.path === path
}
</script>

<style scoped>
.nav-link {
  @apply inline-flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-medium
         text-slate-400 hover:text-slate-100 hover:bg-white/5
         transition-all duration-150;
}

.nav-link-active {
  @apply text-sky-400 bg-sky-500/10;
}
</style>
