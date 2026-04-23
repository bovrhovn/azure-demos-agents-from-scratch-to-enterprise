<template>
  <section aria-live="polite" aria-label="Search results">
    <!-- Error state -->
    <div v-if="error" class="glass-card p-6 border-red-500/20 bg-red-500/5" data-testid="error-state">
      <div class="flex items-start gap-3">
        <div class="shrink-0 w-8 h-8 rounded-full bg-red-500/10 flex items-center justify-center">
          <svg class="w-4 h-4 text-red-400" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126ZM12 15.75h.007v.008H12v-.008Z" />
          </svg>
        </div>
        <div>
          <h3 class="text-sm font-medium text-red-400 mb-1">Search Error</h3>
          <p class="text-sm text-slate-400">{{ error }}</p>
        </div>
      </div>
    </div>

    <!-- Loading skeleton -->
    <div v-else-if="isLoading" class="space-y-3" data-testid="loading-state">
      <div v-for="i in 3" :key="i" class="glass-card p-5 animate-pulse">
        <div class="flex items-start gap-4">
          <div class="flex-1 space-y-2.5">
            <div class="h-2.5 bg-slate-700 rounded w-24" />
            <div class="h-3 bg-slate-700 rounded w-full" />
            <div class="h-3 bg-slate-700 rounded w-4/5" />
          </div>
        </div>
      </div>
    </div>

    <!-- Empty state (after search, no results) -->
    <div
      v-else-if="hasSearched && results.length === 0"
      class="text-center py-16 animate-fade-in"
      data-testid="empty-state"
    >
      <div class="w-16 h-16 mx-auto rounded-full bg-slate-800 flex items-center justify-center mb-4">
        <svg class="w-7 h-7 text-slate-600" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="1.5">
          <path stroke-linecap="round" stroke-linejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
        </svg>
      </div>
      <h3 class="text-slate-400 font-medium mb-1">No results found</h3>
      <p class="text-sm text-slate-600">Try a different search term.</p>
    </div>

    <!-- Results -->
    <div v-else-if="results.length > 0" class="space-y-3" data-testid="results-list">
      <!-- Results header -->
      <div class="flex items-center justify-between mb-4">
        <p class="text-sm text-slate-500">
          <span class="text-slate-300 font-medium">{{ results.length }}</span>
          {{ results.length === 1 ? 'result' : 'results' }} found
        </p>
      </div>

      <ResultCard
        v-for="(result, index) in results"
        :key="index"
        :result="result"
        :highlight="highlight"
      />
    </div>
  </section>
</template>

<script setup lang="ts">
import type { SearchResult } from '@/types/search'
import ResultCard from './ResultCard.vue'

defineProps<{
  results: SearchResult[]
  isLoading: boolean
  error: string | null
  hasSearched: boolean
  highlight?: string
}>()
</script>
