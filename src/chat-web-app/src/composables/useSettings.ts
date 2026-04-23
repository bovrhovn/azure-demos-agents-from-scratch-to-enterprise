import { ref, watch } from 'vue'

const STORAGE_KEY = 'ase_base_url'

// Resolved at runtime: entrypoint.sh substitutes __VITE_API_BASE_URL__ before nginx starts
const ENV_URL = (import.meta.env.VITE_API_BASE_URL as string | undefined) ?? 'https://localhost:5066'

// Module-level singleton so all composable instances share the same reactive state
const baseUrl = ref<string>(localStorage.getItem(STORAGE_KEY) ?? ENV_URL)

watch(baseUrl, (value) => {
  localStorage.setItem(STORAGE_KEY, value)
})

export function useSettings() {
  function saveBaseUrl(url: string): void {
    const trimmed = url.trim()
    baseUrl.value = trimmed || ENV_URL
    localStorage.setItem(STORAGE_KEY, baseUrl.value)
  }

  function resetToDefault(): void {
    baseUrl.value = ENV_URL
    localStorage.removeItem(STORAGE_KEY)
  }

  return {
    baseUrl,
    defaultUrl: ENV_URL,
    saveBaseUrl,
    resetToDefault,
  }
}
