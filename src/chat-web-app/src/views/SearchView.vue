<template>
  <div class="flex flex-col min-h-screen">
    <AppHeader />

    <main class="flex-1 mx-auto w-full max-w-4xl px-6 py-12">
      <!-- Hero section -->
      <div class="text-center mb-12 animate-fade-in">
        <!-- Decorative element -->
        <div class="inline-flex items-center gap-2 px-3 py-1.5 rounded-full bg-sky-500/10 border border-sky-500/20 text-sky-400 text-xs font-medium mb-6">
          <span class="w-1.5 h-1.5 rounded-full bg-sky-400 animate-pulse-slow" />
          Enterprise Search Engine
        </div>

        <h2 class="text-4xl font-semibold text-slate-100 mb-3 tracking-tight">
          Find what you're looking for
        </h2>
        <p class="text-slate-400 text-lg max-w-xl mx-auto">
          Search across enterprise data sources instantly.
        </p>
      </div>

      <!-- Search area -->
      <div class="glass-card p-6 mb-8 animate-slide-up" style="animation-delay: 0.1s;">
        <SearchInput
          v-model="query"
          :is-loading="isLoading"
          :validation-error="validationError"
          @submit="handleSearch"
          @clear-validation="clearValidation"
        />

        <!-- Hints -->
        <div v-if="!hasSearched && !isLoading" class="mt-4 flex flex-wrap gap-2">
          <span class="text-xs text-slate-600">Try:</span>
          <button
            v-for="hint in searchHints"
            :key="hint"
            class="text-xs text-slate-500 hover:text-sky-400 transition-colors duration-150"
            @click="applyHint(hint)"
          >
            "{{ hint }}"
          </button>
        </div>
      </div>

      <!-- Results area -->
      <div v-if="isLoading || hasSearched || error" class="animate-fade-in">
        <SearchResults
          :results="results"
          :is-loading="isLoading"
          :error="error"
          :has-searched="hasSearched"
        />
      </div>

      <!-- Initial state — decorative -->
      <div v-else class="text-center py-8 animate-fade-in" style="animation-delay: 0.2s;">
        <div class="grid grid-cols-3 gap-4 max-w-md mx-auto">
          <div v-for="feature in features" :key="feature.label" class="glass-card p-4 text-center">
            <div class="text-2xl mb-2">{{ feature.icon }}</div>
            <div class="text-xs text-slate-500">{{ feature.label }}</div>
          </div>
        </div>
      </div>
    </main>

    <!-- Footer -->
    <footer class="border-t border-white/5 py-4 px-6">
      <p class="text-center text-xs text-slate-600">
        ASE Enterprise Search · <span class="text-slate-500">{{ apiBaseUrl }}</span>
      </p>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import AppHeader from '@/components/AppHeader.vue'
import SearchInput from '@/components/SearchInput.vue'
import SearchResults from '@/components/SearchResults.vue'
import { useSearch } from '@/composables/useSearch'

const { results, isLoading, error, hasSearched, validationError, search, clearValidation } = useSearch()

const query = ref('')
const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? 'https://localhost:5066'

const searchHints = ['return', 'payment', 'account', 'transfer']

const features = [
  { icon: '⚡', label: 'Fast search' },
  { icon: '🔒', label: 'Secure' },
  { icon: '📊', label: 'Enterprise data' },
]

async function handleSearch(searchQuery: string): Promise<void> {
  await search(searchQuery)
}

function applyHint(hint: string): void {
  query.value = hint
  void handleSearch(hint)
}
</script>
