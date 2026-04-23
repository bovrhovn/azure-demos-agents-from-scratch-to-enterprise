<template>
  <div class="flex flex-col min-h-screen">
    <AppHeader />

    <main class="flex-1 mx-auto w-full max-w-4xl px-6 py-12">
      <!-- Hero -->
      <div class="text-center mb-10 animate-fade-in">
        <div class="inline-flex items-center gap-2 px-3 py-1.5 rounded-full bg-sky-500/10 border border-sky-500/20 text-sky-400 text-xs font-medium mb-6">
          <span class="w-1.5 h-1.5 rounded-full bg-sky-400 animate-pulse-slow" />
          Advanced Search
        </div>
        <h2 class="text-4xl font-semibold text-slate-100 mb-3 tracking-tight">
          Fine-grained search
        </h2>
        <p class="text-slate-400 text-lg max-w-xl mx-auto">
          Filter by source, date range, and result count.
        </p>
      </div>

      <!-- Search form -->
      <div class="glass-card p-6 mb-8 animate-slide-up" style="animation-delay: 0.1s;">
        <form @submit.prevent="handleSearch" class="space-y-5" data-testid="advanced-search-form">

          <!-- Query -->
          <div>
            <label for="adv-query" class="block text-xs text-slate-400 mb-1.5">Search query</label>
            <div class="relative">
              <div class="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                <svg class="w-4 h-4 text-slate-500" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
                  <path stroke-linecap="round" stroke-linejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
                </svg>
              </div>
              <input
                id="adv-query"
                v-model="params.query"
                type="text"
                placeholder="Search enterprise data..."
                class="input-field pl-11"
                :class="{ 'border-red-500 focus:border-red-500 focus:ring-red-500': !!validationError }"
                :disabled="isLoading"
                autocomplete="off"
                spellcheck="false"
                data-testid="advanced-query-input"
                @input="clearValidation"
              />
            </div>
            <Transition name="error">
              <p v-if="validationError" class="mt-1.5 text-sm text-red-400 flex items-center gap-1.5" role="alert">
                <svg class="w-3.5 h-3.5 shrink-0" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-8-5a.75.75 0 01.75.75v4.5a.75.75 0 01-1.5 0v-4.5A.75.75 0 0110 5zm0 10a1 1 0 100-2 1 1 0 000 2z" clip-rule="evenodd" />
                </svg>
                {{ validationError }}
              </p>
            </Transition>
          </div>

          <!-- Filters row -->
          <div class="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <!-- Source filter -->
            <div>
              <label for="adv-source" class="block text-xs text-slate-400 mb-1.5">Source name</label>
              <input
                id="adv-source"
                v-model="params.source"
                type="text"
                placeholder="e.g. confluence"
                class="input-field"
                :disabled="isLoading"
                autocomplete="off"
              />
            </div>

            <!-- Max results -->
            <div>
              <label for="adv-max" class="block text-xs text-slate-400 mb-1.5">Max results</label>
              <select
                id="adv-max"
                v-model.number="params.maxResults"
                class="input-field"
                :disabled="isLoading"
              >
                <option :value="undefined">No limit</option>
                <option :value="5">5</option>
                <option :value="10">10</option>
                <option :value="25">25</option>
                <option :value="50">50</option>
              </select>
            </div>

            <!-- Placeholder column for alignment -->
            <div class="hidden sm:block" />
          </div>

          <!-- Date range row -->
          <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label for="adv-from" class="block text-xs text-slate-400 mb-1.5">From date</label>
              <input
                id="adv-from"
                v-model="params.fromDate"
                type="date"
                class="input-field date-input"
                :disabled="isLoading"
              />
            </div>
            <div>
              <label for="adv-to" class="block text-xs text-slate-400 mb-1.5">To date</label>
              <input
                id="adv-to"
                v-model="params.toDate"
                type="date"
                class="input-field date-input"
                :disabled="isLoading"
              />
            </div>
          </div>

          <!-- Actions -->
          <div class="flex items-center gap-3 pt-1">
            <button
              type="submit"
              class="btn-primary flex items-center gap-2"
              :disabled="isLoading"
              data-testid="advanced-search-button"
            >
              <svg v-if="!isLoading" class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
                <path stroke-linecap="round" stroke-linejoin="round" d="M6 13.5V3.75m0 9.75a1.5 1.5 0 0 1 0 3m0-3a1.5 1.5 0 0 0 0 3m0 3.75V16.5m6-10.5V3.75m0 2.25a1.5 1.5 0 0 1 0 3m0-3a1.5 1.5 0 0 0 0 3m0 9.75V8.25m6 2.25V3.75m0 6.75a1.5 1.5 0 0 1 0 3m0-3a1.5 1.5 0 0 0 0 3m0 3.75V13.5" />
              </svg>
              <svg v-else class="w-4 h-4 animate-spin" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" />
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
              </svg>
              {{ isLoading ? 'Searching…' : 'Advanced Search' }}
            </button>

            <button
              v-if="hasSearched || hasFilters"
              type="button"
              class="text-sm text-slate-400 hover:text-slate-200 transition-colors"
              @click="handleReset"
            >
              Clear
            </button>
          </div>
        </form>
      </div>

      <!-- Results -->
      <div v-if="isLoading || hasSearched || error" class="animate-fade-in">
        <SearchResults
          :results="results"
          :is-loading="isLoading"
          :error="error"
          :has-searched="hasSearched"
        />
      </div>
    </main>

    <footer class="border-t border-white/5 py-4 px-6">
      <p class="text-center text-xs text-slate-600">
        ASE Enterprise Search · <span class="text-slate-500">{{ baseUrl }}</span>
      </p>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { reactive, computed } from 'vue'
import AppHeader from '@/components/AppHeader.vue'
import SearchResults from '@/components/SearchResults.vue'
import { useAdvancedSearch } from '@/composables/useAdvancedSearch'
import { useSettings } from '@/composables/useSettings'
import type { AdvancedSearchParams } from '@/types/search'

const { results, isLoading, error, hasSearched, validationError, search, clearValidation, reset } =
  useAdvancedSearch()
const { baseUrl } = useSettings()

const params = reactive<AdvancedSearchParams>({
  query: '',
  source: '',
  maxResults: undefined,
  fromDate: '',
  toDate: '',
})

const hasFilters = computed(
  () => !!params.source || !!params.maxResults || !!params.fromDate || !!params.toDate,
)

async function handleSearch(): Promise<void> {
  await search({ ...params })
}

function handleReset(): void {
  params.query = ''
  params.source = ''
  params.maxResults = undefined
  params.fromDate = ''
  params.toDate = ''
  reset()
}
</script>

<style scoped>
.error-enter-active,
.error-leave-active {
  transition: all 0.2s ease;
}
.error-enter-from,
.error-leave-to {
  opacity: 0;
  transform: translateY(-4px);
}

/* Style date input calendar icon to match dark theme */
.date-input::-webkit-calendar-picker-indicator {
  filter: invert(0.5);
  cursor: pointer;
}
</style>
