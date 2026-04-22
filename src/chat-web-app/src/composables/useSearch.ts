import { ref, readonly } from 'vue'
import type { SearchResult } from '@/types/search'
import { searchAsync } from '@/services/searchService'

export function useSearch() {
  const results = ref<SearchResult[]>([])
  const isLoading = ref(false)
  const error = ref<string | null>(null)
  const hasSearched = ref(false)
  const validationError = ref<string | null>(null)

  function validate(query: string): boolean {
    const trimmed = query.trim()
    if (!trimmed) {
      validationError.value = 'Please enter a search query.'
      return false
    }
    if (trimmed.length < 2) {
      validationError.value = 'Search query must be at least 2 characters.'
      return false
    }
    validationError.value = null
    return true
  }

  async function search(query: string): Promise<void> {
    if (!validate(query)) return

    isLoading.value = true
    error.value = null
    results.value = []

    try {
      results.value = await searchAsync(query)
      hasSearched.value = true
    } catch (err) {
      const message = err instanceof Error ? err.message : 'An unexpected error occurred.'
      error.value = message.includes('Failed to fetch')
        ? 'Could not reach the search service. Make sure the backend is running.'
        : message
      hasSearched.value = true
    } finally {
      isLoading.value = false
    }
  }

  function clearValidation(): void {
    validationError.value = null
  }

  function reset(): void {
    results.value = []
    isLoading.value = false
    error.value = null
    hasSearched.value = false
    validationError.value = null
  }

  return {
    results: readonly(results),
    isLoading: readonly(isLoading),
    error: readonly(error),
    hasSearched: readonly(hasSearched),
    validationError: readonly(validationError),
    search,
    clearValidation,
    reset,
  }
}
