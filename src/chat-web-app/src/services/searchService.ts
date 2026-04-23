import type { SearchResult, AdvancedSearchParams } from '@/types/search'

export async function searchAsync(query: string, baseUrl: string): Promise<SearchResult[]> {
  const url = `${baseUrl}/basic/search?query=${encodeURIComponent(query.trim())}`

  const response = await fetch(url, {
    method: 'GET',
    headers: { 'Accept': 'application/json' },
  })

  if (!response.ok) {
    throw new Error(`Search failed: ${response.status} ${response.statusText}`)
  }

  return response.json() as Promise<SearchResult[]>
}

export async function advancedSearchAsync(
  params: AdvancedSearchParams,
  baseUrl: string,
): Promise<SearchResult[]> {
  const qs = new URLSearchParams()
  qs.set('query', params.query.trim())
  if (params.source)     qs.set('source', params.source)
  if (params.maxResults) qs.set('maxResults', String(params.maxResults))
  if (params.fromDate)   qs.set('fromDate', params.fromDate)
  if (params.toDate)     qs.set('toDate', params.toDate)

  const url = `${baseUrl}/advanced/search?${qs.toString()}`

  const response = await fetch(url, {
    method: 'GET',
    headers: { 'Accept': 'application/json' },
  })

  if (!response.ok) {
    throw new Error(`Advanced search failed: ${response.status} ${response.statusText}`)
  }

  return response.json() as Promise<SearchResult[]>
}
