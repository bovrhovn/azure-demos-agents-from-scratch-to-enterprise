import type { SearchResult } from '@/types/search'

const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'https://localhost:5066'

export async function searchAsync(query: string): Promise<SearchResult[]> {
  const url = `${BASE_URL}/basic/search?query=${encodeURIComponent(query.trim())}`

  const response = await fetch(url, {
    method: 'GET',
    headers: {
      'Accept': 'application/json',
    },
  })

  if (!response.ok) {
    throw new Error(`Search failed: ${response.status} ${response.statusText}`)
  }

  return response.json() as Promise<SearchResult[]>
}
