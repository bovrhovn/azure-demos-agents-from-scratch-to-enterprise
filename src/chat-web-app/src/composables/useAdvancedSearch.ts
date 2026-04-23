import { ref } from 'vue'
import type { SearchResult, AdvancedSearchParams } from '@/types/search'
import { advancedSearchAsync } from '@/services/searchService'
import { useSettings } from '@/composables/useSettings'

export function useAdvancedSearch() {
  const { baseUrl } = useSettings()
  const results = ref<SearchResult[]>([])
  const isLoading = ref(false)
  const error = ref<string | null>(null)
  const hasSearched = ref(false)
  const validationError = ref<string | null>(null)

  function validate(params: AdvancedSearchParams): boolean {
    const trimmed = params.query.trim()
    if (!trimmed) {
      validationError.value = 'Please enter a search query.'
      return false
    }
    if (trimmed.length < 2) {
      validationError.value = 'Search query must be at least 2 characters.'
      return false
    }
    if (params.fromDate && params.toDate && params.fromDate > params.toDate) {
      validationError.value = '"From" date must be before "To" date.'
      return false
    }
    validationError.value = null
    return true
  }

  async function search(params: AdvancedSearchParams): Promise<void> {
    if (!validate(params)) return

    isLoading.value = true
    error.value = null
    results.value = []

    try {
      results.value = await advancedSearchAsync(params, baseUrl.value)
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
    results,
    isLoading,
    error,
    hasSearched,
    validationError,
    search,
    clearValidation,
    reset,
  }
}
