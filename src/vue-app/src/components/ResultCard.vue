<template>
  <article
    class="glass-card p-5 hover:bg-white/10 transition-all duration-200 group animate-slide-up"
    data-testid="result-card"
  >
    <div class="flex items-start justify-between gap-4">
      <div class="flex-1 min-w-0">
        <!-- Source name -->
        <div class="flex items-center gap-2 mb-2">
          <div class="w-1.5 h-1.5 rounded-full bg-sky-400 shrink-0" />
          <span
            class="text-xs font-medium text-sky-400 tracking-wide uppercase truncate"
            data-testid="result-source"
          >
            {{ result.sourceName }}
          </span>
        </div>

        <!-- Text content -->
        <p class="text-sm text-slate-300 leading-relaxed line-clamp-3" data-testid="result-text">
          {{ result.text }}
        </p>
      </div>

      <!-- Link -->
      <a
        v-if="result.sourceLink"
        :href="result.sourceLink"
        target="_blank"
        rel="noopener noreferrer"
        class="shrink-0 p-2 rounded-lg text-slate-500 hover:text-sky-400 hover:bg-sky-400/10 transition-all duration-200"
        :aria-label="`Open ${result.sourceName}`"
        data-testid="result-link"
      >
        <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
          <path stroke-linecap="round" stroke-linejoin="round" d="M13.5 6H5.25A2.25 2.25 0 0 0 3 8.25v10.5A2.25 2.25 0 0 0 5.25 21h10.5A2.25 2.25 0 0 0 18 18.75V10.5m-10.5 6L21 3m0 0h-5.25M21 3v5.25" />
        </svg>
      </a>
    </div>
  </article>
</template>

<script setup lang="ts">
import type { SearchResult } from '@/types/search'

defineProps<{
  result: SearchResult
}>()
</script>
